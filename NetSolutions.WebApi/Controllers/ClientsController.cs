using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class ClientsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;

    public ClientsController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        IOptions<SmtpSettings> smtpSettings
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings.Value;
    }



    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var clients = await _context.Clients
                .Select(c => new
                {
                    c.Id,
                    c.UserName,
                    c.LastName,
                    c.FirstName,
                    c.Email,
                    c.EmailConfirmed,
                    c.PhoneNumber,
                    c.PhoneNumberConfirmed,
                    c.Gender,
                    c.Bio,
                    c.CreatedAt,
                    c.UpdatedAt,
                    Avatar = c.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                            .Where(ur => ur.UserId == c.Id) // Match roles by user ID
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                            .ToList(), // Get role names as a list
                    c.Subscription,
                    c.Organization,
                    Projects = c.Projects.Select(p => new 
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Team,
                        p.ProjectTasks,
                        p.BusinessService,
                        p.Client,
                        p.ProjectMilestones,
                        p.Budget,
                        Documents = p.Documents.Select(d => d.FileMetadata).ToList(),  // Select FileMetadata from the junction table
                        p.CreatedAt,
                    }).ToList(),
                    c.UserActivities,
                    Solutions = c.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .ToListAsync();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]string Id)
    {
        try
        {
            var client = await _context.Clients
                .Where(c => c.Id == Id)
                .Include(c => c.Organization)
                .Select(c => new
                {
                    c.Id,
                    c.UserName,
                    c.LastName,
                    c.FirstName,
                    c.Email,
                    c.EmailConfirmed,
                    c.PhoneNumber,
                    c.PhoneNumberConfirmed,
                    c.Gender,
                    c.Bio,
                    c.CreatedAt,
                    c.UpdatedAt,
                    Avatar = c.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                            .Where(ur => ur.UserId == c.Id) // Match roles by user ID
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                            .ToList(), // Get role names as a list
                    c.Subscription,
                    c.Organization,
                    Projects = c.Projects.Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Team,
                        p.ProjectTasks,
                        p.BusinessService,
                        p.Client,
                        p.ProjectMilestones,
                        p.Budget,
                        p.CreatedAt,
                        Status = EnumHelper.GetDisplayName(p.Status),
                        Documents = p.Documents.Select(d => d.FileMetadata).ToList(),  // Select FileMetadata from the junction table
                    }).ToList(),
                    c.UserActivities,
                    Solutions = c.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .FirstOrDefaultAsync();
            return Ok(client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
