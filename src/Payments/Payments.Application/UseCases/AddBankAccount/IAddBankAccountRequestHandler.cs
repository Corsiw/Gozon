namespace Payments.Application.UseCases.AddBankAccount
{
    public interface IAddBankAccountRequestHandler
    {
        Task<AddBankAccountResponse> HandleAsync(Guid userId, CancellationToken ct = default);
    }
}