using Domain.Entities;
using Orders.Application.UseCases.AddOrder;
using Orders.Application.UseCases.GetOrderStatusById;
using Orders.Application.UseCases.ListOrders;

namespace Orders.Application.Mappers
{
    public interface IOrderMapper
    {
        Order MapAddOrderRequestToEntity(Guid userId, AddOrderRequest request);
        AddOrderResponse MapEntityToAddOrderResponse(Order work);
        ListOrdersResponseItem MapEntityToListOrdersResponseItem(Order order);
        GetOrderStatusByIdResponse MapEntityToGetOrderByIdResponse(Order order);
        // AnalyzeWorkRequestDto MapEntityToAnalyzeWorkRequest(Work work);
    }
}