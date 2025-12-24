using Payments.Application.Events;

namespace Payments.Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);
        Task MarkAsPublishedByIdAsync(Guid messageId, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<OutboxMessage>> GetPendingOutboxMessagesAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}