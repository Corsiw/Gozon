using Infrastructure.KafkaConsumer.Dto;
using Orders.Application.UseCases.ChangeOrderStatus;

namespace Infrastructure.KafkaConsumer.Mappers
{
    public class ConsumerDtoMapper : IConsumerDtoMapper
    {
        public ChangeOrderStatusRequest MapPaymentDtoToChangeOrderStatusRequest(PaymentDto paymentDto)
        {
            return new ChangeOrderStatusRequest(
                paymentDto.OrderId,
                paymentDto.UserId,
                paymentDto.Amount,
                paymentDto.Status,
                paymentDto.FailureReason,
                paymentDto.OccurredAtUtc
            );
        }
    }
}