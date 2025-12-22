using Microsoft.OpenApi.Models;

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
            // builder.Services.AddDbContext<PaymentsDbContext>(options =>
            // {
            //     string? connectionString = builder.Configuration.GetConnectionString("PaymentsDatabase");
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
            
            // builder.Services.AddScoped<IRepository<Payment>>(sp =>
            // {
            //     PaymentsDbContext dbContext = sp.GetRequiredService<PaymentsDbContext>();
            //     EfRepository<Payment> efRepo = new(dbContext);
            //     return efRepo;
            // });

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
        
            // app.UseMiddleware<ErrorHandlingMiddleware>();
            
            // app.MapPaymentsEndpoints();

            // using (IServiceScope scope = app.Services.CreateScope())
            // { 
            //     PaymentsDbContext db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
            //     db.Database.Migrate();   // создаёт payments.db и таблицы
            // }
            
            app.Run();
        }
    }
}