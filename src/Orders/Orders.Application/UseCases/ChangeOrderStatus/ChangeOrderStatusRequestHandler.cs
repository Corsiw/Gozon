using Domain.Entities;
using Orders.Application.Interfaces;
using Orders.Application.UseCases.GetOrderStatusById;

namespace Orders.Application.UseCases.ChangeOrderStatus
{
    public class ChangeOrderStatusRequestHandler(IOrderRepository repository)
        : IChangeOrderStatusRequestHandler
    {
        public async Task<bool> HandleAsync(ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            bool result = await repository.UpdateStatusAsync(request.OrderId, request.UserId, request.Status, cancellationToken);
            // WARN При изменении функционала (введении транзакции) убрать сохранение в контекст.
            await repository.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}