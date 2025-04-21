using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserSkillsController : ControllerBase
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

    public UserSkillsController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IEmailSender emailSender, 
        ILogger<AccountController> logger, 
        RoleManager<IdentityRole> roleManager, 
        ApplicationDbContext context, 
        IJasonWebToken jasonWebToken, 
        SmtpSettings smtpSettings, 
        IRedisCache redisCache)
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
    }



    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var skills = await _context.UserSkills
                .ToListAsync();
            return Ok(skills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]Guid Id)
    {
        try
        {
            var skills = await _context.UserSkills
                .Where(x => x.Id == Id)
                .ToListAsync();

            return Ok(skills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid Id)
    {
        try
        {
            var affectedRows = await _context.UserSkills
                .Where(x => x.Id == Id)
                .ExecuteDeleteAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
