using Infrastructure.Inbox.Dto;
using Payments.Application.UseCases.ProcessOrder;

namespace Infrastructure.Inbox.Mappers
{
    public class InboxDtoMapper : IInboxDtoMapper
    {
        public ProcessOrderRequest MapOrderDtoToProcessOrderRequest(OrderDto orderDto)
        {
            return new ProcessOrderRequest(
                orderDto.OrderId,
                orderDto.UserId,
                orderDto.Amount
            );
        }
    }
}