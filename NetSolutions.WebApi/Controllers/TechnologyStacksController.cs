using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechnologyStacksController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;

    public TechnologyStacksController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var technologyStacks = await _context.TechnologyStacks
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return Ok(technologyStacks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]Guid Id)
    {
        try
        {
            var technologyStack = await _context.TechnologyStacks
                .AsNoTrackingWithIdentityResolution()
                .Where(ts => ts.Id == Id)
                .FirstOrDefaultAsync();

            return Ok(technologyStack);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }
}
