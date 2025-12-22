using Domain.Enums;

namespace Domain.Entities
{
    public class Order(Guid userId, decimal amount, string? description)
    {
        public Guid OrderId { get; init; } = Guid.NewGuid();
        public Guid UserId { get; init; } = userId;

        public decimal Amount { get; init; } = amount;
        public string? Description { get; init; } = description;

        public OrderStatus Status { get; private set; } = OrderStatus.New;

        public void SetStatusFinished()
        {
            Status = OrderStatus.Finished;
        }

        public void SetStatusCancelled()
        {
            Status = OrderStatus.Cancelled;
        }
    }
}