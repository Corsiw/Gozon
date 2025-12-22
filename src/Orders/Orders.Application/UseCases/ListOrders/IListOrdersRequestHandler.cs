namespace Orders.Application.UseCases.ListOrders
{
    public interface IListOrdersRequestHandler
    {
        Task<ListOrdersResponse> HandleAsync();
    }
}