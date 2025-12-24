using Confluent.Kafka;
using System.Text.Json;

namespace Infrastructure.KafkaConsumer
{
    public sealed class JsonValueDeserializer<T> : IDeserializer<T?>
    {
        public T? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return isNull ? default : JsonSerializer.Deserialize<T?>(data);
        }
    }
}