namespace Payments.Application.Events
{
    public class InboxMessage(Guid messageId)
    {
        private const int MaxRetryCount = 5;

        public Guid MessageId { get; init; } = messageId;

        public DateTime? ProcessedAtUtc { get; private set; }

        public int RetryCount { get; private set; }

        public string? LastError { get; private set; }

        public bool IsProcessed => ProcessedAtUtc != null;

        public bool CanRetry => RetryCount < MaxRetryCount;

        public void IncrementRetry(string error)
        {
            RetryCount++;
            LastError = error;
        }

        public void MarkAsProcessed()
        {
            ProcessedAtUtc = DateTime.UtcNow;
        }
    }
}