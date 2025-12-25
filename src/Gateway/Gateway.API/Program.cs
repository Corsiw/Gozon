namespace Gateway.API
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // YARP ReverseProxy
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigins") ?? string.Empty)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                // Use multiple endpoints for all microservices
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/orders/swagger/v1/swagger.json", "Orders API v1");
                    c.SwaggerEndpoint("/payments/swagger/v1/swagger.json", "Payments API v1");
                });
            }

            app.UseAuthorization();
            
            app.UseCors("Frontend");
            
            app.MapReverseProxy();
            
            app.Run();
        }
    }
}