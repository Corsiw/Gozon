using Domain.Entities;

namespace Payments.Application.Interfaces
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount bankAccount, CancellationToken cancellationToken = default);
        Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}