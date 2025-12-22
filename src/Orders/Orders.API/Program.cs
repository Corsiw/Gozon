using Microsoft.OpenApi.Models;

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
            // builder.Services.AddDbContext<OrdersDbContext>(options =>
            // {
            //     string? connectionString = builder.Configuration.GetConnectionString("OrdersDatabase");
            //     options.UseSqlite(connectionString);
            // });
            
            // Retry Policy
            // builder.Services.AddHttpClient<IFileStorageClient, FileStorageClient>(client =>
            //     {
            //         client.BaseAddress = new Uri("http://filestorage.api:8082");
            //         client.Timeout = TimeSpan.FromSeconds(10);
            //     })
            //     // Retry for transient errors (HTTP 5xx, network failure)
            //     .AddPolicyHandler(HttpPolicyExtensions
            //         .HandleTransientHttpError()
            //         .WaitAndRetryAsync(
            //             retryCount: 3,
            //             sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt)
            //         )
            //     )
            //     // Circuit breaker to stop flooding a failing service
            //     .AddPolicyHandler(HttpPolicyExtensions
            //         .HandleTransientHttpError()
            //         .CircuitBreakerAsync(
            //             handledEventsAllowedBeforeBreaking: 5,
            //             durationOfBreak: TimeSpan.FromSeconds(20)
            //         )
            //     )
            //     // Operation timeout
            //     .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));
            
            // builder.Services.AddScoped<IRepository<Order>>(sp =>
            // {
            //     OrdersDbContext dbContext = sp.GetRequiredService<OrdersDbContext>();
            //     EfRepository<Order> efRepo = new(dbContext);
            //     return efRepo;
            // });

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
        
            // app.UseMiddleware<ErrorHandlingMiddleware>();
            
            // app.MapOrdersEndpoints();

            // using (IServiceScope scope = app.Services.CreateScope())
            // { 
            //     OrdersDbContext db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            //     db.Database.Migrate();   // создаёт orders.db и таблицы
            // }
            
            app.Run();
        }
    }
}