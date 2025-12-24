using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class BankAccountRepository(PaymentsDbContext context) : IBankAccountRepository
    {
        public async Task AddAsync(BankAccount account, CancellationToken cancellationToken = default)
        {
            await context.BankAccounts.AddAsync(account, cancellationToken);
        }

        public async Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.BankAccounts
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId, cancellationToken);
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