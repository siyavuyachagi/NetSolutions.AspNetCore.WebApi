using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

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

    public ProjectsController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings)
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
            var projects = await _context.Projects
                .AsNoTrackingWithIdentityResolution()
                .Include(p => p.Documents)  // Including the junction table
                    .ThenInclude(pd => pd.FileMetadata)  // Include related FileMetadata entities
                .Select(p => new
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
                })
                .ToListAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }


    [HttpDelete("{Id}"), Authorize]
    public async Task<IActionResult> Delete([FromRoute]Guid Id)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
