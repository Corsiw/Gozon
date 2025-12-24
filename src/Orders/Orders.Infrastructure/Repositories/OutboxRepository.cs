using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Events;
using Orders.Application.Interfaces;
using Payments.Application.Exceptions;

namespace Infrastructure.Repositories
{
    public class OutboxRepository(OrdersDbContext context) : IOutboxRepository
    {
        public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
        {
            await context.Messages.AddAsync(message, cancellationToken);
        }

        public async Task MarkAsPublishedByIdAsync(Guid messageId, CancellationToken cancellationToken = default)
        {
            await context.Messages
                .Where(x => x.MessageId == messageId)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        x => x.ProcessedAtUtc,
                        _ => DateTime.UtcNow),
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await context.Messages.Where(o => o.ProcessedAtUtc == null).ToListAsync(cancellationToken);
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