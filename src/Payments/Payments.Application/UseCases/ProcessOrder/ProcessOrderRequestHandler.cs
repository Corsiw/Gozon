using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Payments.Application.Events;
using Payments.Application.Exceptions;
using Payments.Application.Interfaces;
using System.Text.Json;

namespace Payments.Application.UseCases.ProcessOrder
{
    public class ProcessOrderRequestHandler(
        IBankAccountRepository repository,
        IOutboxRepository outboxRepository)
        : IProcessOrderRequestHandler
    {
        private const int MaxRetries = 3;

        public async Task<PaymentStatus> HandleAsync(
            ProcessOrderRequest request,
            CancellationToken ct = default)
        {
            decimal amount = request.Amount;

            PaymentStatus status = default;
            string? failureReason = null;

            if (amount <= 0)
            {
                status = PaymentStatus.Cancelled;
                failureReason = "InsufficientFunds";
            }
            else
            {
                for (int attempt = 1; attempt <= MaxRetries && !ct.IsCancellationRequested; attempt++)
                {
                    try
                    {
                        BankAccount? account =
                            await repository.GetByUserIdAsync(request.UserId, ct);

                        if (account is null)
                        {
                            status = PaymentStatus.Cancelled;
                            failureReason = "AccountNotFound";
                            break;
                        }

                        account.SubtractAmount(amount);

                        status = PaymentStatus.Finished;
                        break;
                    }
                    catch (InsufficientFundsException)
                    {
                        status = PaymentStatus.Cancelled;
                        failureReason = "InsufficientFunds";
                        break;
                    }
                    catch (OverflowException)
                    {
                        status = PaymentStatus.Cancelled;
                        failureReason = "Overflow";
                        break;
                    }
                    catch (ConcurrencyConflictException) when (attempt < MaxRetries)
                    {
                        await Task.Delay(20 * attempt, ct);
                    }
                }
            }
            
            if (status == default)
            {
                failureReason = "ConcurrencyConflict";
            }

            // ВСЕГДА публикуем событие
            OrderStatusChangedEvent evt = new(
                OrderId: request.OrderId,
                UserId: request.UserId,
                Amount: amount,
                Status: status,
                FailureReason: failureReason,
                OccurredAtUtc: DateTime.UtcNow
            );
            
            Console.WriteLine(evt);

            await outboxRepository.AddAsync(
                new OutboxMessage(
                    "PaymentStatusChanged",
                    JsonSerializer.Serialize(evt)
                ),
                ct);

            return status;
        }
    }
}