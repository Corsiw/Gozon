using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Payments.Application.Exceptions;

namespace Infrastructure.Repositories
{
    public class OrderRepository(OrdersDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(Guid orderId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.Orders
                .FirstOrDefaultAsync(o =>
                    o.OrderId == orderId &&
                    o.UserId == userId, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await context.Orders.AddAsync(order, cancellationToken);
        }

        public async Task<bool> UpdateStatusAsync(
            Guid orderId,
            Guid userId,
            OrderStatus newStatus,
            CancellationToken cancellationToken = default)
        {
            int affected = await context.Orders
                .Where(o => o.OrderId == orderId && o.UserId == userId)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(o => o.Status, newStatus),
                    cancellationToken);

            return affected > 0;
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            try
            {
                await context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyConflictException(ex.Message, ex);
            }
        }
    }
}