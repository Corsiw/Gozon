using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payments.Application.Events;
using Payments.Application.Interfaces;

namespace Infrastructure.Outbox
{
    public class OutboxPublisherService(IServiceProvider serviceProvider, ILogger<OutboxPublisherService> logger)
        : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            IOutboxRepository repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            IKafkaProducer producer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();

            while (!ct.IsCancellationRequested)
            {
                IReadOnlyCollection<OutboxMessage> messages = await repository.GetPendingOutboxMessagesAsync(ct);

                foreach (OutboxMessage msg in messages)
                {
                    try
                    {
                        await producer.PublishAsync(
                            msg.Type,
                            msg.MessageId,
                            msg.Payload,
                            ct);

                        await repository.MarkAsPublishedByIdAsync(msg.MessageId, ct);
                        await repository.SaveChangesAsync(ct);
                    }
                    catch (InvalidOperationException)
                    {
                        logger.LogError("Outbox message with id {MsgMessageId} was not published.", msg.MessageId);
                    }
                }

                await Task.Delay(10000, ct);
            }
        }
    }
}