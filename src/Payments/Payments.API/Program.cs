using Confluent.Kafka;
using Infrastructure.Data;
using Infrastructure.Inbox;
using Infrastructure.Inbox.Dto;
using Infrastructure.Inbox.Mappers;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Payments.API.Endpoints;
using Payments.API.Middleware;
using Payments.Application.Interfaces;
using Payments.Application.Mappers;
using Payments.Application.UseCases.AddBankAccount;
using Payments.Application.UseCases.CreditBankAccount;
using Payments.Application.UseCases.GetBalanceByUserId;
using Payments.Application.UseCases.ProcessOrder;

namespace Payments.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payments API", Version = "v1" });

                // Указываем basePath, который будет отображаться в Swagger UI
                c.AddServer(new OpenApiServer
                {
                    Url = "/payments", // здесь префикс Gateway
                    Description = "Access via API Gateway"
                });
            });
            
            // Configure Sqlite in appsettings.json
            builder.Services.AddDbContext<PaymentsDbContext>(options =>
            {
                string? connectionString = builder.Configuration.GetConnectionString("PaymentsDatabase");
                options.UseSqlite(connectionString);
            });
            
            builder.Services.AddScoped<IBankAccountRepository>(sp =>
            {
                PaymentsDbContext dbContext = sp.GetRequiredService<PaymentsDbContext>();
                BankAccountRepository efRepo = new(dbContext);
                return efRepo;
            });
            builder.Services.AddScoped<IInboxRepository>(sp =>
            {
                PaymentsDbContext dbContext = sp.GetRequiredService<PaymentsDbContext>();
                InboxRepository efRepo = new(dbContext);
                return efRepo;
            });
            builder.Services.AddScoped<IOutboxRepository>(sp =>
            {
                PaymentsDbContext dbContext = sp.GetRequiredService<PaymentsDbContext>();
                OutboxRepository efRepo = new(dbContext);
                return efRepo;
            });
            builder.Services.AddScoped<IUnitOfWork>(sp =>
            {
                PaymentsDbContext dbContext = sp.GetRequiredService<PaymentsDbContext>();
                EfUnitOfWork efuow = new(dbContext);
                return efuow;
            });

            builder.Services.AddScoped<IAddBankAccountRequestHandler, AddBankAccountRequestHandler>();
            builder.Services.AddScoped<IGetBankAccountBalanceByUserIdRequestHandler, GetBankAccountBalanceByUserIdRequestHandler>();
            builder.Services.AddScoped<ICreditBankAccountRequestHandler, CreditBankAccountRequestHandler>();
            builder.Services.AddScoped<IProcessOrderRequestHandler, ProcessOrderRequestHandler>();
            
            builder.Services.AddSingleton<IBankAccountMapper, BankAccountMapper>();
            
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
                builder.Configuration.GetSection("Kafka:Consumers:Payments"));
            builder.Services.AddSingleton<IConsumer<Ignore, OrderDto?>>(sp =>
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

                return new ConsumerBuilder<Ignore, OrderDto?>(config)
                    .SetValueDeserializer(new JsonValueDeserializer<OrderDto>()!)
                    .Build();
            });

            builder.Services.AddSingleton<IInboxDtoMapper, InboxDtoMapper>();
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments.API V1");
                });
            }
        
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.MapPaymentsEndpoints();

            using (IServiceScope scope = app.Services.CreateScope())
            { 
                PaymentsDbContext db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
                db.Database.Migrate();   // создаёт payments.db и таблицы
            }
            
            app.Run();
        }
    }
}