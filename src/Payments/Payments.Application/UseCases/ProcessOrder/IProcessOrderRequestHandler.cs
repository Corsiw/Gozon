using Domain.Enums;
using Payments.Application.UseCases.CreditBankAccount;

namespace Payments.Application.UseCases.ProcessOrder
{
    public interface IProcessOrderRequestHandler
    {
        Task<PaymentStatus> HandleAsync(ProcessOrderRequest request, CancellationToken ct = default);
    }
}