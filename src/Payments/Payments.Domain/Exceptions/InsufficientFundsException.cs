namespace Domain.Exceptions
{
    public class InsufficientFundsException(string? message = null) : Exception(message);
}