namespace Payments.Application.UseCases.AddBankAccount
{
    public record AddBankAccountResponse
    (
        Guid BankAccountId,
        Guid UserId,
        decimal Amount
    );
}