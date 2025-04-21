using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SystemLogEntriesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public SystemLogEntriesController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IEmailSender emailSender, 
        ILogger<AccountController> logger, 
        RoleManager<IdentityRole> roleManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var logEntries = await _context.SystemLogEntries
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return Ok(logEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]Guid Id)
    {
        try
        {
            var logEntry = await _context.SystemLogEntries
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            return Ok(logEntry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid Id)
    {
        try
        {
            var logEntry = await _context.SystemLogEntries
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (logEntry is null) return NoContent();

            _context.SystemLogEntries.Remove(logEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        try
        {
            var logEntries = await _context.SystemLogEntries
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return Ok(logEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}
