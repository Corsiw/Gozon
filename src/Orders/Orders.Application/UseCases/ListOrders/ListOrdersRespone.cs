namespace Orders.Application.UseCases.ListOrders
{
    public record ListOrdersResponseItem
    (
        Guid OrderId,
        Guid UserId,
        decimal Amount,
        string? Description,
        string Status
    );

    public record ListOrdersResponse(IReadOnlyList<ListOrdersResponseItem> Orders);
}