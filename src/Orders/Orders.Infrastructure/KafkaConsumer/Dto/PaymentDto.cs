using Domain.Enums;

namespace Infrastructure.KafkaConsumer.Dto
{
    public sealed record PaymentDto(
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        OrderStatus Status,
        string? FailureReason,
        DateTime OccurredAtUtc
    );
}