namespace Payments.Application.UseCases.GetBalanceByUserId
{
    public interface IGetBankAccountBalanceByUserIdRequestHandler
    {
        Task<GetBankAccountBalanceByUserIdResponse?> HandleAsync(Guid userId);
    }
}