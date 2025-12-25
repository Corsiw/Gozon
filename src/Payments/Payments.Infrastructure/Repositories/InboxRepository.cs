using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Events;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class InboxRepository(PaymentsDbContext context) : IInboxRepository
    {
        public async Task AddAsync(InboxMessage message, CancellationToken cancellationToken = default)
        {
            await context.InboxMessages.AddAsync(message, cancellationToken);
        }

        public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
        {
            InboxMessage? message = await context.InboxMessages
                .FirstOrDefaultAsync(m => m.MessageId == messageId, cancellationToken);

            if (message == null)
            {
                throw new NotFoundException($"Inbox message with id {messageId} not found.");
            }

            message.MarkAsProcessed();
        }

        public async Task<InboxMessage?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken = default)
        {
             return await context.InboxMessages
                 .FirstOrDefaultAsync(m => m.MessageId == messageId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<InboxMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await context.InboxMessages.Where(o => o.ProcessedAtUtc == null).ToListAsync(cancellationToken);
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