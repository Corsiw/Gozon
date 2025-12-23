using Domain.Entities;
using Payments.Application.UseCases.AddBankAccount;

namespace Payments.Application.Mappers
{
    public class BankAccountMapper : IBankAccountMapper
    {
        public BankAccount MapAddWorkRequestToEntity(Guid userId)
        {
            return new BankAccount(userId);
        }
        
        public AddBankAccountResponse MapEntityToAddOrderResponse(BankAccount account)
        {
            return new AddBankAccountResponse(
                account.BankAccountId,
                account.UserId,
                account.Amount
            );
        }
        
        // public GetOrderStatusByIdResponse MapEntityToGetWorkByIdResponse(Order order)
        // {
        //     return new GetOrderStatusByIdResponse(
        //         order.OrderId,
        //         order.UserId,
        //         order.Status.ToString()
        //     );
        // }
        //
        // public AnalyzeWorkRequestDto MapEntityToAnalyzeWorkRequest(Work work)
        // {
        //     return new AnalyzeWorkRequestDto(
        //         work.WorkId,
        //         work.FileId ?? throw new NotFoundException("File not attached"),
        //         work.StudentId,
        //         work.AssignmentId,
        //         work.SubmissionTime
        //     );
        // }
        //
        // public ListOrdersResponseItem MapEntityToListOrdersResponseItem(Order order)
        // {
        //     return new ListOrdersResponseItem(
        //         order.OrderId,
        //         order.UserId,
        //         order.Amount,
        //         order.Description,
        //         order.Status.ToString()
        //     );
        // }
    }
}