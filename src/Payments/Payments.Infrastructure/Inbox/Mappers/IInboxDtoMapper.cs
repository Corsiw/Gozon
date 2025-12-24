using Infrastructure.Inbox.Dto;
using Payments.Application.UseCases.ProcessOrder;

namespace Infrastructure.Inbox.Mappers
{
    public interface IInboxDtoMapper
    {
        ProcessOrderRequest MapOrderDtoToProcessOrderRequest(OrderDto orderDto);
    }
}