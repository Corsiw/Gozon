namespace Infrastructure.Inbox.Dto
{
    public sealed record OrderDto(
        Guid OrderId,
        Guid UserId,
        decimal Amount
    );
}