using Confluent.Kafka;
using Domain.Enums;
using Infrastructure.Inbox.Dto;
using Infrastructure.Inbox.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payments.Application.Events;
using Payments.Application.Interfaces;
using Payments.Application.UseCases.ProcessOrder;

namespace Infrastructure.Inbox
{
    public class OrdersConsumer(
        IServiceProvider serviceProvider,
        IInboxDtoMapper mapper,
        IConsumer<Ignore, OrderDto?> consumer,
        ILogger<OrdersConsumer> logger)
        : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("OrdersConsumer started.");
            
            consumer.Subscribe("OrderCreated");
            logger.LogInformation("Subscribed to Kafka topic: OrderCreated");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, OrderDto?>? result;

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

                IInboxRepository repository = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
                IProcessOrderRequestHandler handler =
                    scope.ServiceProvider.GetRequiredService<IProcessOrderRequestHandler>();
                IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                Guid messageId = result.Message.Value.OrderId;
                logger.LogInformation("Processing order {OrderId} {UserId} {Amount}.", messageId,  result.Message.Value.UserId, result.Message.Value.Amount);

                try
                {
                    await unitOfWork.BeginAsync(cancellationToken);

                    InboxMessage? inbox = await repository.GetByIdAsync(messageId, cancellationToken);

                    if (inbox != null && inbox.ProcessedAtUtc != null)
                    {
                        // Уже обработано, безопасно пропускаем
                        await unitOfWork.CommitAsync(cancellationToken);
                        consumer.Commit(result);
                        logger.LogInformation("Message {MessageId} already processed, skipping.", messageId);
                        continue;
                    }

                    if (inbox == null)
                    {
                        inbox = new InboxMessage(messageId);
                        await repository.AddAsync(inbox, cancellationToken);
                        logger.LogInformation("New inbox message created: {MessageId}", messageId);
                    }

                    PaymentStatus resultingStatus = await handler.HandleAsync(
                        mapper.MapOrderDtoToProcessOrderRequest(result.Message.Value),
                        cancellationToken);

                    inbox.MarkAsProcessed();
                    await repository.SaveChangesAsync(cancellationToken);
                    await unitOfWork.CommitAsync(cancellationToken);

                    consumer.Commit(result);
                    logger.LogInformation("Message {MessageId} processed successfully. Status: {ResultingStatus}", messageId, resultingStatus);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message {MessageId}. Reason: {Reason}", messageId, ex.Message);
                    await unitOfWork.RollbackAsync(cancellationToken);
                }

                // Небольшая пауза, чтобы избежать спама при ошибках
                await Task.Delay(100, cancellationToken);
            }

            // Закрываем consumer при остановке сервиса
            consumer.Close();
        }
    }
}