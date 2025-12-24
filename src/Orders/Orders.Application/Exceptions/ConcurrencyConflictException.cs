namespace Payments.Application.Exceptions
{
    public class ConcurrencyConflictException(string message, Exception ex) : Exception($"Concurrent modification detected. {message}", ex);
}