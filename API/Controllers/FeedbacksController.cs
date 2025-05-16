using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly ILogger<FeedbacksController> _logger;
        private readonly IFeedback _feedbackRepository;

        public FeedbacksController(
            ILogger<FeedbacksController> logger,
            IFeedback feedbackRepository)
        {
            _logger = logger;
            _feedbackRepository = feedbackRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            try
            {
                var result = await _feedbackRepository.GetFeedbackAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            try
            {
                await _feedbackRepository.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
