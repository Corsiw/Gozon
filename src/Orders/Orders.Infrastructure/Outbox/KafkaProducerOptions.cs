namespace Infrastructure.Outbox
{
    public class KafkaProducerOptions
    {
        public required string Topic { get; init; }
        public required string BootstrapServers { get; init; }
    }
}