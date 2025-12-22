namespace Orders.Application.UseCases.GetOrderStatusById
{
    public record GetOrderStatusByIdResponse
    (
        Guid OrderId,
        Guid UserId,
        string Status
    );
}