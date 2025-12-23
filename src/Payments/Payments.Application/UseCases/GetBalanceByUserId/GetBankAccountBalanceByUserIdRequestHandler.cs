using Domain.Entities;
using Payments.Application.Interfaces;
using Payments.Application.Mappers;

namespace Payments.Application.UseCases.GetBalanceByUserId
{
    public class GetBankAccountBalanceByUserIdRequestHandler(IBankAccountRepository repository, IBankAccountMapper mapper)
        : IGetBankAccountBalanceByUserIdRequestHandler
    {
        public async Task<GetBankAccountBalanceByUserIdResponse?> HandleAsync(Guid userId)
        {
            BankAccount? bankAccount = await repository.GetByUserIdAsync(userId);
            return bankAccount == null ? null : mapper.MapEntityToGetBankAccountBalanceByUserIdResponse(bankAccount);
        }
    }
}