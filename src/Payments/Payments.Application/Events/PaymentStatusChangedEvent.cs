using Domain.Enums;

namespace Payments.Application.Events
{
    public sealed record PaymentStatusChangedEvent(
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        PaymentStatus Status,
        string? FailureReason,
        DateTime OccurredAtUtc
    );
}