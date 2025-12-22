using Domain.Entities;
using Orders.Application.UseCases.ListOrders;

namespace Orders.Application.Mappers
{
    public interface IOrderMapper
    {
        // Work MapAddWorkRequestToEntity(AddWorkRequest request);
        // AddWorkResponse MapEntityToAddWorkResponse(Work work);
        ListOrdersResponseItem MapEntityToListOrdersResponseItem(Order order);
        // GetWorkByIdResponse MapEntityToGetWorkByIdResponse(Work work);
        // AnalyzeWorkRequestDto MapEntityToAnalyzeWorkRequest(Work work);
    }
}