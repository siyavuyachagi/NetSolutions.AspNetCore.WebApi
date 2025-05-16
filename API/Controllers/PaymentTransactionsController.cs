using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionsController : ControllerBase
    {
        private readonly ILogger<PaymentTransactionsController> _logger;
        private readonly IPaymentTransaction _paymentTransaction;

        public PaymentTransactionsController(
            ILogger<PaymentTransactionsController> logger, 
            IPaymentTransaction paymentTransaction)
        {
            _logger = logger;
            _paymentTransaction = paymentTransaction;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = _paymentTransaction.GetPaymentTransactionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            try
            {
                var result = await _paymentTransaction.GetPaymentTransactionAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _paymentTransaction.DeletePaymentTransactionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}

