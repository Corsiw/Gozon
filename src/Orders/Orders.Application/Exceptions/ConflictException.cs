namespace Infrastructure.Exceptions
{
    public class ConflictException(string? message = null) : Exception(message);
}