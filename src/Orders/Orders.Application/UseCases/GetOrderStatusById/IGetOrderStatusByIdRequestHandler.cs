namespace Orders.Application.UseCases.GetOrderStatusById
{
    public interface IGetOrderStatusByIdRequestHandler
    {
        Task<GetOrderStatusByIdResponse?> HandleAsync(Guid orderId, Guid userId);
    }
}