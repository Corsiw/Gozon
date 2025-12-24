namespace Orders.Application.Events
{
    public class OutboxMessage(string type, string payload)
    {
        public Guid MessageId { get; init; } = Guid.NewGuid();
        
        public string Type { get; init; } = type;
        public string Payload { get; init; } = payload;
        
        public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
        public DateTime? ProcessedAtUtc { get; private set; }
        
        public int RetryCount { get; private set; }

        public void MarkAsPublished()
        {
            ProcessedAtUtc = DateTime.UtcNow;
        }
    }
}