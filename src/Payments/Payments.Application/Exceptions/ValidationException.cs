namespace Payments.Application.Exceptions
{
    public class ValidationException(string? message = null) : Exception(message);
}