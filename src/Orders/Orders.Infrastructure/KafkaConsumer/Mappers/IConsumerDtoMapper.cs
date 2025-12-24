using Infrastructure.KafkaConsumer.Dto;
using Orders.Application.UseCases.ChangeOrderStatus;

namespace Infrastructure.KafkaConsumer.Mappers
{
    public interface IConsumerDtoMapper
    {
        ChangeOrderStatusRequest MapPaymentDtoToChangeOrderStatusRequest(PaymentDto paymentDto);
    }
}