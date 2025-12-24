using Domain.Enums;

namespace Orders.Application.UseCases.ChangeOrderStatus
{
    public sealed record ChangeOrderStatusRequest(
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        OrderStatus Status,
        string? FailureReason,
        DateTime OccurredAtUtc
    );
}