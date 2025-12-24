namespace Infrastructure.Inbox
{
    public class PaymentsConsumerOptions
    {
        public required string Topic { get; init; }
        public required string BootstrapServers { get; init; }
        public required string GroupId { get; init; } = "payments-service";
    }
}