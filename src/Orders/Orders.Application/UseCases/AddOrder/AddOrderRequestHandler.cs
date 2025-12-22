using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.AddOrder
{
    public class AddOrderRequestHandler(IOrderRepository repository, IOrderMapper mapper) : IAddOrderRequestHandler
    {
        public async Task<AddOrderResponse> HandleAsync(Guid userId, AddOrderRequest request)
        {
            Order order = mapper.MapAddWorkRequestToEntity(userId, request);

            await repository.AddAsync(order);
            
            // TODO Anync message to Payments

            return mapper.MapEntityToAddOrderResponse(order);
        }
    }
}