using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Events;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;
using System.Collections.Immutable;

namespace Infrastructure.Repositories
{
    public class OutboxRepository(PaymentsDbContext context) : IOutboxRepository
    {
        public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
        {
            await context.OutboxMessages.AddAsync(message, cancellationToken);
        }

        public async Task MarkAsPublishedByIdAsync(Guid messageId, CancellationToken cancellationToken = default)
        {
            await context.OutboxMessages
                .Where(x => x.MessageId == messageId)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        x => x.ProcessedAtUtc,
                        _ => DateTime.UtcNow),
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await context.OutboxMessages.Where(o => o.ProcessedAtUtc == null).ToListAsync(cancellationToken);
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