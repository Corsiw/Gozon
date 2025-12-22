namespace Orders.Application.UseCases.AddOrder
{
    public record AddOrderRequest(
        Guid UserId,
        decimal Amount,
        string? Description
    );
}