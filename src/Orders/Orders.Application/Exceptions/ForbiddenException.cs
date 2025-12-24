namespace Infrastructure.Exceptions
{
    public class ForbiddenException(string? message = null) : Exception(message);
}