namespace Infrastructure.Exceptions
{
    public class UnauthorizedException(string? message = null) : Exception(message);
}