using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;
using Domain.Validations;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationUsersController : ControllerBase
{
    private readonly ILogger<ApplicationUsersController> _logger;
    private readonly IApplicationUser _applicationUserRepository;

    public ApplicationUsersController(
        ILogger<ApplicationUsersController> logger,
        IApplicationUser applicationUserRepository)
    {
        _logger = logger;
        _applicationUserRepository = applicationUserRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _applicationUserRepository.GetApplicationUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] string id)
    {
        try
        {
            var user = await _applicationUserRepository.GetApplicationUserAsync(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }


    public class ProfileImageModel
    {
        [FileSizeMax(5)]
        public IFormFile ImageFile { get; set; }

    }
    [HttpPatch("{Id}/profile/image")]
    public async Task<IActionResult> UpdateProfileImage([FromRoute] string Id, ProfileImageModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem();

            await _applicationUserRepository.EditProfileImage(Id, model.ImageFile);
            return Ok(await _applicationUserRepository.GetApplicationUserAsync(Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    public class UpdateBioModel
    {
        public string? Bio { get; set; }
    }
    [HttpPatch("{Id}/update/bio")]
    public async Task<IActionResult> UpdateBio([FromRoute] string Id, [FromBody]UpdateBioModel model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _applicationUserRepository.EditBio(Id, model.Bio);
            return Ok(await _applicationUserRepository.GetApplicationUserAsync(Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}
