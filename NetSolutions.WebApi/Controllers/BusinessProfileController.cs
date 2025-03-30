using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public BusinessProfileController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessProfile = _configuration.GetSection(nameof(BusinessProfile)).Get<BusinessProfile>();
                return Ok(businessProfile);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
