namespace Payments.Application.UseCases.CreditBankAccount
{
    public interface ICreditBankAccountRequestHandler
    {
        Task<CreditBankAccountResponse> HandleAsync(Guid userId, CreditBankAccountRequest request, CancellationToken ct = default);
    }
}