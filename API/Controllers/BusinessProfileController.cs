using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessProfileController : ControllerBase
    {
        private readonly IBusinessProfile _businessProfileRepository;
        private readonly ILogger<BusinessProfileController> _logger;

        public BusinessProfileController(
            IBusinessProfile businessProfileRepository,
            ILogger<BusinessProfileController> logger)
        {
            _businessProfileRepository = businessProfileRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessProfile = await _businessProfileRepository.GetBusinessProfileAsync();
                return Ok(businessProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
