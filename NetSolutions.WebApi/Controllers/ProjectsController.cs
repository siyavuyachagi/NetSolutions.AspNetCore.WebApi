using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Repositories;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly IRedisCache _redisCache;
    private readonly IProjectRepository _projectsRepository;
    public ProjectsController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        IRedisCache redisCache,
        IProjectRepository projectsRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
        _redisCache = redisCache;
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
            //await _projectsRepository.DeleteProjectAsync(Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    //[HttpGet("staff/{Id}")]
    //public async Task<IActionResult> GetProjectsByStaffId([FromRoute] string Id)
    //{
    //    try
    //    {
    //        var projects = await _projectsRepository.GetProjectsAsync();

    //        if (!projects.Succeeded || projects.Response == null)
    //            return BadRequest("Unable to retrieve projects.");

    //        var userProjects = projects.Response
    //            .Where(p => p.ProjectTeam.ProjectTeamMembers
    //                .Any(m => m.UserId == Id)) // Adjust `UserId` if named differently
    //            .ToList();

    //        return Ok(userProjects);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, ex.Message);
    //        return StatusCode(500, ex.Message);
    //    }
    //}

}
