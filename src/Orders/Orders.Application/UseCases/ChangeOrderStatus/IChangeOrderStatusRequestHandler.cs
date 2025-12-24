using Orders.Application.UseCases.GetOrderStatusById;

namespace Orders.Application.UseCases.ChangeOrderStatus
{
    public interface IChangeOrderStatusRequestHandler
    {
        Task<bool> HandleAsync(ChangeOrderStatusRequest request, CancellationToken cancellationToken);
    }
}