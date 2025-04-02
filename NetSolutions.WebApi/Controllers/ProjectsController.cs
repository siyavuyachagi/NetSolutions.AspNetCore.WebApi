using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;

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
                .Include(p => p.Client)
                .ThenInclude(c => c.Organization)
                .Include(p => p.Documents)  // Including the junction table
                    .ThenInclude(pd => pd.FileMetadata)  // Include related FileMetadata entities
                .Include(p => p.Team)
                    .ThenInclude(t => t.TeamMembers)
                        .ThenInclude(tm => tm.Member)
                .Include(p => p.Solutions.Where(s => !s.IsDeleted))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Budget,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.BusinessService,
                    p.Client,
                    p.ProjectTasks,
                    p.ProjectMilestones,
                    p.Solutions,
                    Status = EnumHelper.GetDisplayName(p.Status),
                    Documents = p.Documents.Select(d => d.FileMetadata).ToList(),  // Select FileMetadata from the junction table
                    TechnologyStacks = p.TechnologyStacks.Select(ts => ts.TechnologyStack).ToList(),
                    Team = new
                    {
                        p.Team.Id,
                        p.Team.Name,
                        TeamMembers = p.Team.TeamMembers.Select(tm =>new
                        {
                            tm.Id,
                            tm.CreatedAt,
                            Roles = tm.Roles.Select(r => r.TeamMemberRole).ToList(),
                            tm.Member,
                        })
                    },
                })
                .ToListAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute]Guid Id)
    {
        try
        {
            var projects = await _context.Projects
                .AsNoTrackingWithIdentityResolution()
                .Where(p => p.Id == Id)
                .Include(p => p.Client)
                .ThenInclude(c => c.Organization)
                .Include(p => p.Documents)  // Including the junction table
                    .ThenInclude(pd => pd.FileMetadata)  // Include related FileMetadata entities
                .Include(p => p.Team)
                    .ThenInclude(t => t.TeamMembers)
                        .ThenInclude(tm => tm.Member)
                .Include(p => p.Solutions.Where(s => !s.IsDeleted))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Budget,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.BusinessService,
                    p.Client,
                    p.ProjectTasks,
                    p.ProjectMilestones,
                    p.Solutions,
                    Status = EnumHelper.GetDisplayName(p.Status),
                    Documents = p.Documents.Select(d => d.FileMetadata).ToList(),  // Select FileMetadata from the junction table
                    TechnologyStacks = p.TechnologyStacks.Select(ts => ts.TechnologyStack).ToList(),
                    Team = new
                    {
                        p.Team.Id,
                        p.Team.Name,
                        TeamMembers = p.Team.TeamMembers.Select(tm => new
                        {
                            tm.Id,
                            tm.CreatedAt,
                            Roles = tm.Roles.Select(r => r.TeamMemberRole).ToList(),
                            tm.Member,
                        })
                    },
                })
                .FirstOrDefaultAsync();
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
            await _context.Projects
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsDeleted, true));
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
