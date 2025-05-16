using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessServicesController : ControllerBase
    {
        private readonly ILogger<BusinessServicesController> _logger;
        private readonly IBusinessService _businessServiceRepository;

        public BusinessServicesController(
            ILogger<BusinessServicesController> logger, 
            IBusinessService businessServiceRepository)
        {
            _logger = logger;
            _businessServiceRepository = businessServiceRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessServices = await _businessServiceRepository.GetBusinessServicesAsync();
                return Ok(businessServices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Guid Id)
        {
            try
            {
                if (!ModelState.IsValid) return NotFound();

                var businessService = await _businessServiceRepository.GetBusinessServiceAsync(Id);
                return Ok(businessService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }



        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid Id)
        {
            try
            {
                await _businessServiceRepository.DeleteBusinessServiceAsync(Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
