using Domain.Entities;
using Payments.Application.UseCases.AddBankAccount;
using Payments.Application.UseCases.CreditBankAccount;
using Payments.Application.UseCases.GetBalanceByUserId;

namespace Payments.Application.Mappers
{
    public interface IBankAccountMapper
    {
        BankAccount MapAddBankAccountRequestToEntity(Guid userId);
        AddBankAccountResponse MapEntityToAddBankAccountResponse(BankAccount account);
        GetBankAccountBalanceByUserIdResponse MapEntityToGetBankAccountBalanceByUserIdResponse(BankAccount account);
        CreditBankAccountResponse MapEntityToCreditBankAccountResponse(BankAccount account);
    }
}