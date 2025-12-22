using Domain.Entities;
using Orders.Application.UseCases.AddOrder;
using Orders.Application.UseCases.GetOrderStatusById;
using Orders.Application.UseCases.ListOrders;

namespace Orders.Application.Mappers
{
    public interface IOrderMapper
    {
        Order MapAddWorkRequestToEntity(AddOrderRequest request);
        AddOrderResponse MapEntityToAddOrderResponse(Order work);
        ListOrdersResponseItem MapEntityToListOrdersResponseItem(Order order);
        GetOrderStatusByIdResponse MapEntityToGetWorkByIdResponse(Order order);
        // AnalyzeWorkRequestDto MapEntityToAnalyzeWorkRequest(Work work);
    }
}