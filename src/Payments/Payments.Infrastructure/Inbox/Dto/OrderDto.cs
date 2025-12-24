namespace Infrastructure.Inbox.Dto
{
    public record OrderDto(
        Guid OrderId,
        Guid UserId,
        decimal Amount
    );
}