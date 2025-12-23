namespace Payments.Application.Exceptions
{
    public class ConcurrencyConflictException() : Exception("Concurrent modification detected.");
}