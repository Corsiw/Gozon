using Domain.Entities;
using Orders.Application.UseCases.ListOrders;

namespace Orders.Application.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        // public Work MapAddWorkRequestToEntity(AddWorkRequest request)
        // {
        //     return new Work(request.StudentId, request.AssignmentId);
        // }
        //
        // public AddWorkResponse MapEntityToAddWorkResponse(Work work)
        // {
        //     return new AddWorkResponse(
        //         work.WorkId,
        //         work.StudentId,
        //         work.AssignmentId,
        //         work.SubmissionTime,
        //         work.Status.ToString()
        //     );
        // }
        //
        // public GetWorkByIdResponse MapEntityToGetWorkByIdResponse(Work work)
        // {
        //     return new GetWorkByIdResponse(
        //         work.WorkId,
        //         work.StudentId,
        //         work.AssignmentId,
        //         work.SubmissionTime,
        //         work.FileId,
        //         work.Status.ToString(),
        //         work.ReportId,
        //         work.PlagiarismFlag
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
        
        public ListOrdersResponseItem MapEntityToListOrdersResponseItem(Order order)
        {
            return new ListOrdersResponseItem(
                order.OrderId,
                order.UserId,
                order.Amount,
                order.Description,
                order.Status.ToString()
            );
        }
    }
}