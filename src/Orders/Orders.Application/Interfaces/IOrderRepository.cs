using Domain.Entities;
using Domain.Enums;

namespace Orders.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);

        Task<bool> UpdateStatusAsync(
            Guid orderId,
            Guid userId,
            OrderStatus newStatus,
            CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken ct);
    }
}