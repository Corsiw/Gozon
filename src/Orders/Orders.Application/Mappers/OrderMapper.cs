using Domain.Entities;
using Orders.Application.UseCases.AddOrder;
using Orders.Application.UseCases.GetOrderStatusById;
using Orders.Application.UseCases.ListOrders;

namespace Orders.Application.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        public Order MapAddWorkRequestToEntity(Guid userId, AddOrderRequest request)
        {
            return new Order(userId, request.Amount, request.Description);
        }
        
        public AddOrderResponse MapEntityToAddOrderResponse(Order order)
        {
            return new AddOrderResponse(
                order.OrderId,
                order.UserId,
                order.Amount,
                order.Description,
                order.Status.ToString()
            );
        }
        //
        public GetOrderStatusByIdResponse MapEntityToGetWorkByIdResponse(Order order)
        {
            return new GetOrderStatusByIdResponse(
                order.OrderId,
                order.UserId,
                order.Status.ToString()
            );
        }
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