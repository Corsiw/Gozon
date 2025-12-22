using Domain.Entities;

namespace Orders.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId);
        Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId);
        Task AddAsync(Order order);
    }
}