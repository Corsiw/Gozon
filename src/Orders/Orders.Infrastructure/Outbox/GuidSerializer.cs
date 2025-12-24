using Confluent.Kafka;

namespace Infrastructure.Outbox
{
    public sealed class GuidSerializer : ISerializer<Guid>
    {
        public byte[] Serialize(Guid data, SerializationContext context)
        {
            return data.ToByteArray();
        }
    }

}