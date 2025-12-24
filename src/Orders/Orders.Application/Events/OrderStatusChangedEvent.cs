using Domain.Enums;

namespace Orders.Application.Events
{
    public sealed record OrderStatusChangedEvent(
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        OrderStatus Status,
        string? FailureReason,
        DateTime OccurredAtUtc
    );
}