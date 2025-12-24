namespace Payments.Application.UseCases.ProcessOrder
{
    public record ProcessOrderRequest(
        Guid OrderId,
        Guid UserId,
        decimal Amount
    );
}