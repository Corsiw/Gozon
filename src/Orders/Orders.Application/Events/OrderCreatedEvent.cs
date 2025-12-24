using Domain.Enums;

namespace Orders.Application.Events
{
    public sealed record OrderCreatedEvent(
        Guid OrderId,
        Guid UserId,
        decimal Amount
    );
}