namespace Infrastructure.Data
{
    public class InboxMessage
    {
        public Guid MessageId { get; init; } = Guid.NewGuid();
        public DateTime? ProcessedAtUtc { get; set; }
    }
}