using Infrastructure.Exceptions;
using Polly.Timeout;
using System.Net;

namespace Orders.API.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // DEVELOPMENT ONLY 
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = exception switch
            {
                NotFoundException =>
                    HttpStatusCode.NotFound,
            
                ValidationException =>
                    HttpStatusCode.BadRequest,
            
                ConflictException =>
                    HttpStatusCode.Conflict,
            
                ForbiddenException =>
                    HttpStatusCode.Forbidden,
            
                UnauthorizedException =>
                    HttpStatusCode.Unauthorized,
                
                // Таймауты внешних сервисов -> 504
                TimeoutRejectedException => HttpStatusCode.GatewayTimeout,

                TaskCanceledException { CancellationToken.IsCancellationRequested: false } =>
                    HttpStatusCode.GatewayTimeout,

                OperationCanceledException { CancellationToken.IsCancellationRequested: false } =>
                    HttpStatusCode.GatewayTimeout,

                // Сетевая ошибка -> внешний сервис недоступен -> 503
                HttpRequestException => HttpStatusCode.ServiceUnavailable,
            
                _ =>
                    HttpStatusCode.InternalServerError
            };
            
            
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(exception.ToString());
        }
    }
}