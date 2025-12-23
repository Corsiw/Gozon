using Domain.Enums;

namespace Domain.Entities
{
    public class PaymentTransaction(Guid orderId, Guid userId, decimal amount)
    {
        public Guid OrderId { get; init; } = orderId;
        public Guid UserId { get; init; } = userId;
        public decimal Amount { get; init; } = amount;

        public PaymentStatus Status { get; private set; } = PaymentStatus.New;

        public void SetStatusFinished()
        {
            Status = PaymentStatus.Finished;
        }

        public void SetStatusCancelled()
        {
            Status = PaymentStatus.Cancelled;
        }
    }
}