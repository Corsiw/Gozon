using Domain.Entities;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;
using Payments.Application.Mappers;

namespace Payments.Application.UseCases.AddBankAccount
{
    public class AddBankAccountRequestHandler(IBankAccountRepository repository, IBankAccountMapper mapper) : IAddBankAccountRequestHandler
    {
        public async Task<AddBankAccountResponse> HandleAsync(Guid userId, CancellationToken ct = default)
        {
            BankAccount? existing = await repository.GetByUserIdAsync(userId, ct);
            if (existing != null)
            {
                throw new ConflictException($"Account for user {userId} already exists");
            }

            BankAccount account = mapper.MapAddBankAccountRequestToEntity(userId);
            await repository.AddAsync(account, ct);
            await repository.SaveChangesAsync(ct);
            
            return mapper.MapEntityToAddBankAccountResponse(account);
        }
    }
}