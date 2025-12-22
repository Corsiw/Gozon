using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.ListOrders
{
    public class ListOrdersRequestHandler(IOrderRepository repository, IOrderMapper mapper) : IListOrdersRequestHandler
    {
        public async Task<ListOrdersResponse> HandleAsync(Guid userId)
        {
            IEnumerable<Order> orders = await repository.ListByUserAsync(userId);
            List<ListOrdersResponseItem> items = orders.Select(mapper.MapEntityToListOrdersResponseItem).ToList();
            return new ListOrdersResponse(items);
        }
    }
}