using Confluent.Kafka;
using Infrastructure.Data;
using Infrastructure.KafkaConsumer;
using Infrastructure.KafkaConsumer.Dto;
using Infrastructure.KafkaConsumer.Mappers;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Orders.API.Endpoints;
using Orders.API.Middleware;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;
using Orders.Application.UseCases.AddOrder;
using Orders.Application.UseCases.ChangeOrderStatus;
using Orders.Application.UseCases.GetOrderStatusById;
using Orders.Application.UseCases.ListOrders;

namespace Orders.API
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SupportNonNullableReferenceTypes();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });

                // Указываем basePath, который будет отображаться в Swagger UI
                c.AddServer(new OpenApiServer
                {
                    Url = "/orders", // здесь префикс Gateway
                    Description = "Access via API Gateway"
                });
            });

            // Configure Sqlite in appsettings.json
            builder.Services.AddDbContext<OrdersDbContext>(options =>
            {
                string? connectionString = builder.Configuration.GetConnectionString("OrdersDatabase");
                options.UseSqlite(connectionString);
            });

            builder.Services.AddScoped<IOrderRepository>(sp =>
            {
                OrdersDbContext dbContext = sp.GetRequiredService<OrdersDbContext>();
                OrderRepository efRepo = new(dbContext);
                return efRepo;
            });
            builder.Services.AddScoped<IOutboxRepository>(sp =>
            {
                OrdersDbContext dbContext = sp.GetRequiredService<OrdersDbContext>();
                OutboxRepository efRepo = new(dbContext);
                return efRepo;
            });
            builder.Services.AddScoped<IUnitOfWork>(sp =>
            {
                OrdersDbContext dbContext = sp.GetRequiredService<OrdersDbContext>();
                EfUnitOfWork efuow = new(dbContext);
                return efuow;
            });

            builder.Services.AddScoped<IListOrdersRequestHandler, ListOrdersRequestHandler>();
            builder.Services.AddScoped<IAddOrderRequestHandler, AddOrderRequestHandler>();
            builder.Services.AddScoped<IGetOrderStatusByIdRequestHandler, GetOrderStatusByIdRequestHandler>();
            builder.Services.AddScoped<IChangeOrderStatusRequestHandler, ChangeOrderStatusRequestHandler>();

            builder.Services.AddSingleton<IOrderMapper, OrderMapper>();

            // Kafka
            builder.Services.Configure<KafkaProducerOptions>(
                builder.Configuration.GetSection("Kafka:Producer"));
            builder.Services.AddSingleton<IProducer<Guid, string>>(sp =>
            {
                KafkaProducerOptions options = sp.GetRequiredService<IOptions<KafkaProducerOptions>>().Value;

                ProducerConfig config = new()
                {
                    BootstrapServers = options.BootstrapServers,
                    EnableIdempotence = true,
                    Acks = Acks.All,
                    MessageSendMaxRetries = int.MaxValue
                };

                return new ProducerBuilder<Guid, string>(config)
                    .SetKeySerializer(new GuidSerializer())
                    .SetValueSerializer(Serializers.Utf8)
                    .Build();
            });

            builder.Services.Configure<PaymentsConsumerOptions>(
                builder.Configuration.GetSection("Kafka:Consumers:Orders"));
            builder.Services.AddSingleton<IConsumer<Ignore, PaymentDto?>>(sp =>
            {
                PaymentsConsumerOptions options = sp.GetRequiredService<IOptions<PaymentsConsumerOptions>>().Value;

                ConsumerConfig config = new()
                {
                    BootstrapServers = options.BootstrapServers,
                    GroupId = options.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                    EnableAutoOffsetStore = false,
                    IsolationLevel = IsolationLevel.ReadCommitted
                };

                return new ConsumerBuilder<Ignore, PaymentDto?>(config)
                    .SetValueDeserializer(new JsonValueDeserializer<PaymentDto>())
                    .Build();
            });

            builder.Services.AddSingleton<IConsumerDtoMapper, ConsumerDtoMapper>();
            builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

            builder.Services.AddHostedService<OutboxPublisherService>();
            builder.Services.AddHostedService<PaymentsConsumer>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders.API V1");
                });
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapOrdersEndpoints();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                OrdersDbContext db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
                db.Database.Migrate(); // создаёт orders.db и таблицы
            }

            app.Run();
        }
    }
}