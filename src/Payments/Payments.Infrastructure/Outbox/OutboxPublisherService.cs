using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Events;
using Payments.Application.Interfaces;

namespace Infrastructure.Outbox
{
    public class OutboxPublisherService(IServiceProvider serviceProvider)
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
                    }
                }

                await Task.Delay(10000, ct);
            }
        }
    }
}