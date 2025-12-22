namespace Orders.Application.UseCases.AddOrder
{
    public record AddOrderResponse
    (
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        string? Description,
        string Status
    );
}