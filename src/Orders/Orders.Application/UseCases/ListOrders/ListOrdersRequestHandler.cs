using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;

namespace Orders.Application.UseCases.ListOrders
{
    public class ListOrdersRequestHandler(IRepository<Order> repository, IOrderMapper mapper) : IListOrdersRequestHandler
    {
        public async Task<ListOrdersResponse> HandleAsync()
        {
            IEnumerable<Order> orders = await repository.ListAsync();
            List<ListOrdersResponseItem> items = orders.Select(mapper.MapEntityToListOrdersResponseItem).ToList();
            return new ListOrdersResponse(items);
        }
    }
}