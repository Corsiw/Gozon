using Domain.Entities;
using Payments.Application.UseCases.AddBankAccount;
using Payments.Application.UseCases.CreditBankAccount;
using Payments.Application.UseCases.GetBalanceByUserId;
using Payments.Application.UseCases.ProcessOrder;

namespace Payments.Application.Mappers
{
    public class BankAccountMapper : IBankAccountMapper
    {
        public BankAccount MapAddBankAccountRequestToEntity(Guid userId)
        {
            return new BankAccount(userId);
        }

        public AddBankAccountResponse MapEntityToAddBankAccountResponse(BankAccount account)
        {
            return new AddBankAccountResponse(
                account.BankAccountId,
                account.UserId,
                account.Amount
            );
        }

        public GetBankAccountBalanceByUserIdResponse MapEntityToGetBankAccountBalanceByUserIdResponse(
            BankAccount account)
        {
            return new GetBankAccountBalanceByUserIdResponse(
                account.BankAccountId,
                account.UserId,
                account.Amount
            );
        }

        public CreditBankAccountResponse MapEntityToCreditBankAccountResponse(BankAccount account)
        {
            return new CreditBankAccountResponse(
                account.Amount
            );
        }
    }
}