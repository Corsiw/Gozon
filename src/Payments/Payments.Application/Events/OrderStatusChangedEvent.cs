using Domain.Enums;

namespace Payments.Application.Events
{
    public sealed record OrderStatusChangedEvent(
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        PaymentStatus Status,
        string? FailureReason,
        DateTime OccurredAtUtc
    );
}