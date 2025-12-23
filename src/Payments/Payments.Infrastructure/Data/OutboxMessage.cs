namespace Infrastructure.Data
{
    public class OutboxMessage(string type, string payload)
    {
        public Guid MessageId { get; init; } = Guid.NewGuid();
        
        public string Type { get; init; } = type;
        public string Payload { get; init; } = payload;
        
        public DateTime OccurredAtUtc { get; init; }
        public DateTime? ProcessedAtUtc { get; set; }
        
        public int RetryCount { get; set; }
    }
}