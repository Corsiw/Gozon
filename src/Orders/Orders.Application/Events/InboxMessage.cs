namespace Orders.Application.Events
{
    public class InboxMessage(Guid messageId)
    {
        public Guid MessageId { get; init; } = messageId;
        public DateTime? ProcessedAtUtc { get; private set; }

        public void MarkAsProcessed()
        {
            ProcessedAtUtc = DateTime.UtcNow;
        }
    }
}