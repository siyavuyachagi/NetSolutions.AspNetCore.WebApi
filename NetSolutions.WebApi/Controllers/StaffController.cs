using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly IMapper _mapper;

    public StaffController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var staff = await _context.Staff
                .Include(x => x.PhysicalAddress)
                .Include(x => x.Organization)
                .Include(x => x.UserActivities)
                .Include(x => x.Profession)
                .Include(x => x.Staff_UserSkills)
                .ThenInclude(x => x.UserSkill)
                .ToListAsync();

            if (staff is null) return NotFound();

            var dto = _mapper.Map<List<StaffDto>>(staff);

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] string Id)
    {
        try
        {
            var staff = await _context.Staff
                .Where(x => x.Id == Id)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.Organization)
                .Include(x => x.UserActivities)
                .Include(x => x.Profession)
                .Include(x => x.Staff_UserSkills)
                .ThenInclude(x => x.UserSkill)
                .FirstOrDefaultAsync();

            if (staff is null) return NotFound();

            var dto = _mapper.Map<StaffDto>(staff);

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }
}
