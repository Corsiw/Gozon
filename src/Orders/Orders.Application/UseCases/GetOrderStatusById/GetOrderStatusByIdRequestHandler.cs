using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.GetOrderStatusById
{
    public class GetOrderStatusByIdRequestHandler(IOrderRepository repository, IOrderMapper mapper)
        : IGetOrderStatusByIdRequestHandler
    {
        public async Task<GetOrderStatusByIdResponse?> HandleAsync(Guid orderId, Guid userId)
        {
            Order? order = await repository.GetByIdAsync(orderId, userId);
            return order == null ? null : mapper.MapEntityToGetOrderByIdResponse(order);
        }
    }
}