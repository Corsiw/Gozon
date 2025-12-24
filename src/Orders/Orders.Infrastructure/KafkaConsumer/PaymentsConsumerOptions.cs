namespace Infrastructure.KafkaConsumer
{
    public class PaymentsConsumerOptions
    {
        public required string Topic { get; init; }
        public required string BootstrapServers { get; init; }
        public required string GroupId { get; init; } = "orders-service";
    }
}