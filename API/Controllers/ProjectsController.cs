using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IProject _projectsRepository;
        public ProjectsController(
            ILogger<ProjectsController> logger,
            IProject projectsRepository)
        {
            _logger = logger;
            _projectsRepository = projectsRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var projectsDto = await _projectsRepository.GetProjectsAsync();
                return Ok(projectsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Guid Id)
        {
            try
            {
                var projectDto = await _projectsRepository.GetProjectAsync(Id);
                return Ok(projectDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            try
            {
                await _projectsRepository.DeleteProjectAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }

}

