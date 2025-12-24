using Domain.Entities;
using Microsoft.Extensions.Logging;
using Orders.Application.Events;
using Orders.Application.Interfaces;
using Orders.Application.Mappers;
using System.Text.Json;

namespace Orders.Application.UseCases.AddOrder
{
    public class AddOrderRequestHandler(
        IOrderRepository repository,
        IOutboxRepository outboxRepository,
        IUnitOfWork unitOfWork,
        IOrderMapper mapper,
        ILogger<AddOrderRequestHandler> logger) : IAddOrderRequestHandler
    {
        public async Task<AddOrderResponse> HandleAsync(Guid userId, AddOrderRequest request,
            CancellationToken cancellationToken = default)
        {
            Order order = mapper.MapAddOrderRequestToEntity(userId, request);

            await unitOfWork.BeginAsync(cancellationToken);

            try
            {
                await repository.AddAsync(order, cancellationToken);

                OrderCreatedEvent evt = new(
                    order.OrderId,
                    order.UserId,
                    order.Amount
                );
                await outboxRepository.AddAsync(
                    new OutboxMessage(
                        "OrderCreated",
                        JsonSerializer.Serialize(evt)
                    ),
                    cancellationToken);

                await unitOfWork.CommitAsync(cancellationToken);
                logger.LogInformation("Successfully added order");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync(cancellationToken);
                logger.LogError(ex, "Exception while AddOrder Transaction. Rolling back. Reason: {Message}",
                    ex.Message);
            }

            return mapper.MapEntityToAddOrderResponse(order);
        }
    }
}