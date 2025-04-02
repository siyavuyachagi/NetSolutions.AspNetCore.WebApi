using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationUsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly JwtSettings _jwtSettings;

    public ApplicationUsersController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
        _jwtSettings = jwtSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.LastName,
                    u.FirstName,
                    u.Email,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.Gender,
                    u.Bio,
                    u.CreatedAt,
                    u.UpdatedAt,
                    Avatar = u.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id) // Match roles by user ID
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                        .ToList(), // Get role names as a list
                    u.Organization,
                    u.UserActivities,
                    Solutions = u.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]string Id)
    {
        try
        {
            var users = await _userManager.Users
                .Where(u => u.Id == Id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.LastName,
                    u.FirstName,
                    u.Email,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.Gender,
                    u.Bio,
                    u.CreatedAt,
                    u.UpdatedAt,
                    Avatar = u.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id) // Match roles by user ID
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                        .ToList(), // Get role names as a list
                    u.Organization,
                    u.UserActivities,
                    Solutions = u.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
