using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransaction
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentTransactionRepository> _logger;
        private readonly IMapper _mapper;

        public PaymentTransactionRepository(
            ApplicationDbContext context, 
            ILogger<PaymentTransactionRepository> logger, 
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaymentTransactionDto> CreatePaymentTransactionAsync(PaymentTransaction paymentTransaction)
        {
            try
            {
                _context.PaymentTransactions.Add(paymentTransaction);
                await _context.SaveChangesAsync();
                var result = await GetPaymentTransactionAsync(paymentTransaction.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeletePaymentTransactionAsync(Guid id)
        {
            try
            {
                await _context.PaymentTransactions.Where(x => x.Id == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PaymentTransactionDto> GetPaymentTransactionAsync(Guid id)
        {
            try
            {
                var paymentTransaction = await _context.PaymentTransactions
                    .AsNoTrackingWithIdentityResolution()
                    .Where(pt => pt.Id == id)
                    .FirstOrDefaultAsync();

                var result = _mapper.Map<PaymentTransactionDto>(paymentTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<PaymentTransactionDto>> GetPaymentTransactionsAsync()
        {
            try
            {
                var paymentTransactions = await _context.PaymentTransactions
                    .ToListAsync();

                var result = _mapper.Map<List<PaymentTransactionDto>>(paymentTransactions);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdatePaymentTransactionAsync(Guid id, string? transactionReference = null, string? additionalNotes = null)
        {
            try
            {
                var paymentTransaction = await _context.PaymentTransactions.FindAsync(id);
                if (paymentTransaction is null) return;
                paymentTransaction.TransactionReference = transactionReference;
                paymentTransaction.AdditionalNotes = additionalNotes;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStatusAsync(Guid id, PaymentTransaction.EStatus status)
        {
            try
            {
                var paymentTransaction = await _context.PaymentTransactions.FindAsync(id);
                if (paymentTransaction is null) return;
                paymentTransaction.Status = status;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
