namespace Payments.Application.UseCases.GetBalanceByUserId
{
    public record GetBankAccountBalanceByUserIdResponse(
        Guid BankAccountId,
        Guid UserId,
        decimal Balance
    );
}