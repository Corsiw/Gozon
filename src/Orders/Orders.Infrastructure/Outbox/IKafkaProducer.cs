namespace Infrastructure.Outbox
{
    public interface IKafkaProducer
    {
        Task PublishAsync(
            string topic,
            Guid messageId,
            string payload,
            CancellationToken ct = default);
    }
}