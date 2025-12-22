using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.GetOrderStatusById
{
    public class GetOrderStatusByIdRequestHandler(IRepository<Order> repository, IOrderMapper mapper)
        : IGetOrderStatusByIdRequestHandler
    {
        public async Task<GetOrderStatusByIdResponse?> HandleAsync(Guid orderId)
        {
            Order? order = await repository.GetAsync(orderId);
            return order == null ? null : mapper.MapEntityToGetWorkByIdResponse(order);
        }
    }
}