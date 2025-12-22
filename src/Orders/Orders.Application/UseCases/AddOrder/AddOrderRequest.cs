namespace Orders.Application.UseCases.AddOrder
{
    public record AddOrderRequest(
        decimal Amount,
        string? Description
    );
}