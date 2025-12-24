using Confluent.Kafka;

namespace Infrastructure.Outbox
{
    public sealed class KafkaProducer(IProducer<Guid, string> producer) : IKafkaProducer, IDisposable
    {
        public async Task PublishAsync(
            string topic,
            Guid messageId,
            string payload,
            CancellationToken ct = default)
        {
            Message<Guid, string> message = new() { Key = messageId, Value = payload };

            try
            {
                await producer.ProduceAsync(topic, message, ct);
            }
            catch (ProduceException<Guid, string> ex)
            {
                throw new InvalidOperationException(
                    $"Kafka publish failed: {ex.Error.Reason}", ex);
            }
        }

        public void Dispose()
        {
            producer.Flush(TimeSpan.FromSeconds(5));
            producer.Dispose();
        }
    }
}