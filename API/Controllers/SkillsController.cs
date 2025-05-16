using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ILogger<SkillsController> _logger;
    private readonly ISkills _skills;

    public SkillsController(
        ILogger<SkillsController> logger,
        ISkills skills)
    {
        _logger = logger;
        _skills = skills;
    }



    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _skills.GetSkillsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        try
        {
            var result = await _skills.GetSkillAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _skills.DeleteSkillAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    public class SkillModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SkillModel model)
    {
        try
        {
            var skill = new Skill
            {
                Name = model.Name,
                Description = model.Description
            };

            var result = await _skills.CreateSkillAsync(skill);
            return Created(result.Id.ToString(), result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
