using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnologyStacksController : ControllerBase
    {
        private readonly ILogger<TechnologyStacksController> _logger;
        private readonly ITechnologyStack _technologyStackInterface;

        public TechnologyStacksController(
            ILogger<TechnologyStacksController> logger,
            ITechnologyStack technologyStackInterface)
        {
            _logger = logger;
            _technologyStackInterface = technologyStackInterface;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _technologyStackInterface.GetTechnologyStacksAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var result = await _technologyStackInterface.GetTechnologyStackAsync(id);
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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                await _technologyStackInterface.DeleteTechnologyStackAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
                throw;
            }
        }


        public class TechnologyStackModel
        {
            [Required]
            public string Name { get; set; }
            public string? Description { get; set; }
            public string? NameAbbr { get; set; }
            public string? IconUrl { get; set; }
            public string? IconHTML { get; set; }
            public TechnologyStack.EType Type { get; set; }  // Added Type property
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TechnologyStackModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var technologyStack = new TechnologyStack
                {
                    Name = model.Name,
                    Description = model.Description,
                    NameAbbr = model.NameAbbr,
                    IconUrl = model.IconUrl,
                    IconHTML = model.IconHTML,
                    Type = model.Type,
                };

                var result = await _technologyStackInterface.CreateTechnologyStackAsync(technologyStack);
                return Ok(result);
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

