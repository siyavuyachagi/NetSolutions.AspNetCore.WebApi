using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IClient _clientRepository;

        public ClientsController(
            ILogger<ClientsController> logger,
            IClient clientRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var clients = await _clientRepository.GetClientsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] string Id)
        {
            try
            {
                var client = await _clientRepository.GetClientsAsync();
                return Ok(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}

