using Domain.Entities;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;
using Payments.Application.Mappers;
using Payments.Application.UseCases.AddBankAccount;

namespace Payments.Application.UseCases.CreditBankAccount
{
    public class CreditBankAccountRequestHandler(IBankAccountRepository repository, IBankAccountMapper mapper)
        : ICreditBankAccountRequestHandler
    {
        private const int MaxRetries = 3;

        public async Task<CreditBankAccountResponse> HandleAsync(Guid userId, CreditBankAccountRequest request,
            CancellationToken ct = default)
        {
            decimal amount = request.Amount;
            if (amount <= 0)
            {
                throw new ValidationException("Top-up amount must be positive");
            }

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    BankAccount account = await repository.GetByUserIdAsync(userId, ct)
                                          ?? throw new NotFoundException("Account not found");

                    account.AddAmount(amount);

                    await repository.SaveChangesAsync(ct);

                    return mapper.MapEntityToCreditBankAccountResponse(account);
                }
                catch (ConcurrencyConflictException) when (attempt < MaxRetries)
                {
                    await Task.Delay(20 * attempt, ct);
                }
            }

            throw new ConflictException(
                "Concurrent modification of bank account detected. Please retry.");
        }
    }
}