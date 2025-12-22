using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class OrderRepository(OrdersDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid orderId, Guid userId)
        {
            return await context.Orders
                .FirstOrDefaultAsync(o =>
                    o.OrderId == orderId &&
                    o.UserId == userId);
        }

        public async Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId)
        {
            return await context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
    }

}