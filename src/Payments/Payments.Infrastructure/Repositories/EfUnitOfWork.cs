using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Payments.Application.Interfaces;

namespace Infrastructure.Repositories
{
    public sealed class EfUnitOfWork(PaymentsDbContext context) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public async Task BeginAsync(CancellationToken ct)
        {
            _transaction = await context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
            await _transaction!.CommitAsync(ct);
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            await _transaction!.RollbackAsync(ct);
        }
    }
}