using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly ILogger<OrganizationsController> _logger;
        private readonly IOrganization _organization;

        public OrganizationsController(
            ILogger<OrganizationsController> logger, 
            IOrganization organization)
        {
            _logger = logger;
            _organization = organization;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var organizations = await _organization.GetOrganizationsAsync();
                return Ok(organizations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
