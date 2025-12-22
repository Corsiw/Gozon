namespace Orders.Application.UseCases.AddOrder
{
    public interface IAddOrderRequestHandler
    {
        Task<AddOrderResponse> HandleAsync(Guid userId, AddOrderRequest request);
    }
}