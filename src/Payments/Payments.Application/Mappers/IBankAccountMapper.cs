using Domain.Entities;
using Payments.Application.UseCases.AddBankAccount;

namespace Payments.Application.Mappers
{
    public interface IBankAccountMapper
    {
        BankAccount MapAddWorkRequestToEntity(Guid userId);
        AddBankAccountResponse MapEntityToAddOrderResponse(BankAccount account);
        // ListOrdersResponseItem MapEntityToListOrdersResponseItem(Bank order);
        // GetOrderStatusByIdResponse MapEntityToGetWorkByIdResponse(Bank order);
        // AnalyzeWorkRequestDto MapEntityToAnalyzeWorkRequest(Work work);
    }
}