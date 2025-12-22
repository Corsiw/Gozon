namespace Orders.Application.UseCases.ListOrders
{
    public record ListOrdersResponseItem
    (
        Guid Order,
        Guid UserId,
        decimal Amount,
        string? Description,
        string Status
    );

    public record ListOrdersResponse(IReadOnlyList<ListOrdersResponseItem> Orders);
}