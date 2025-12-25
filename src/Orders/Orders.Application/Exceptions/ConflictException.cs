namespace Orders.Application.Exceptions
{
    public class ConflictException(string? message = null) : Exception(message);
}