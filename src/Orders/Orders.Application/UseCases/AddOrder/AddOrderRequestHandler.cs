using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.AddOrder
{
    public class AddOrderRequestHandler(IRepository<Order> repository, IOrderMapper mapper) : IAddOrderRequestHandler
    {
        public async Task<AddOrderResponse> HandleAsync(AddOrderRequest request)
        {
            Order order = mapper.MapAddWorkRequestToEntity(request);

            await repository.AddAsync(order);
            
            // TODO Anync message to Payments

            return mapper.MapEntityToAddOrderResponse(order);
        }
    }
}