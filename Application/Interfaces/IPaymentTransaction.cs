using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPaymentTransaction
    {
        Task<List<PaymentTransactionDto>> GetPaymentTransactionsAsync();
        Task<PaymentTransactionDto> GetPaymentTransactionAsync(Guid id);
        Task<PaymentTransactionDto> CreatePaymentTransactionAsync(PaymentTransaction paymentTransaction);
        Task UpdatePaymentTransactionAsync(Guid id, string? transactionReference = default, string? additionalNotes = default);
        Task DeletePaymentTransactionAsync(Guid id);
        Task UpdateStatusAsync(Guid id, PaymentTransaction.EStatus status);
    }
}
