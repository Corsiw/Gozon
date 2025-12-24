using Payments.Application.Events;

namespace Payments.Application.Interfaces
{
    public interface IInboxRepository
    {
        Task AddAsync(InboxMessage message, CancellationToken cancellationToken = default);
        Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default);
        Task<InboxMessage?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<InboxMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}