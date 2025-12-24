using Confluent.Kafka;
using Infrastructure.KafkaConsumer.Dto;
using Infrastructure.KafkaConsumer.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orders.Application.UseCases.ChangeOrderStatus;

namespace Infrastructure.KafkaConsumer
{
    public class PaymentsConsumer(
        IServiceProvider serviceProvider,
        IConsumerDtoMapper mapper,
        IConsumer<Ignore, PaymentDto?> consumer,
        ILogger<PaymentsConsumer> logger)
        : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("PaymentsConsumer started.");
            
            consumer.Subscribe("PaymentStatusChanged");
            logger.LogInformation("Subscribed to Kafka topic: PaymentStatusChanged");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, PaymentDto?>? result;

                try
                {
                    result = await Task.Run(() => consumer.Consume(cancellationToken), cancellationToken);
                }
                catch (ConsumeException ex)
                {
                    logger.LogError(ex, "Kafka consume error: {ErrorReason}", ex.Error.Reason);
                    await Task.Delay(1000, cancellationToken);
                    continue;
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (result?.Message?.Value is null)
                {
                    continue;
                }

                // Создаём scope для каждого сообщения
                using IServiceScope scope = serviceProvider.CreateScope();
                
                IChangeOrderStatusRequestHandler handler =
                    scope.ServiceProvider.GetRequiredService<IChangeOrderStatusRequestHandler>();

                Guid messageId = result.Message.Value.OrderId;
                logger.LogInformation("Processing PaymentStatusChanged {OrderId} {UserId} {Status}.", messageId,  result.Message.Value.UserId, result.Message.Value.Status);

                try
                {
                    await handler.HandleAsync(
                        mapper.MapPaymentDtoToChangeOrderStatusRequest(result.Message.Value),
                        cancellationToken);

                    consumer.Commit(result);
                    logger.LogInformation("Message {MessageId} processed successfully.", messageId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message {MessageId}. Reason: {Reason}", messageId, ex.Message);
                }

                // Небольшая пауза, чтобы избежать спама при ошибках
                await Task.Delay(100, cancellationToken);
            }

            // Закрываем consumer при остановке сервиса
            consumer.Close();
        }
    }
}