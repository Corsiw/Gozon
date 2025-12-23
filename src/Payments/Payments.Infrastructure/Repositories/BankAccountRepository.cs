using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class BankAccountRepository(PaymentsDbContext context) : IBankAccountRepository
    {
        public async Task AddAsync(BankAccount account, CancellationToken cancellationToken = default)
        {
            context.BankAccounts.Add(account);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.BankAccounts
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId, cancellationToken);
        }
    }

}