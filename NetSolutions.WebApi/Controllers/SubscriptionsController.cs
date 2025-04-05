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
public class SubscriptionsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly IPayFast _payFast;
    private readonly PayFastCreds _payFastCreds;
    private readonly JwtSettings _jwtSettings;

    public SubscriptionsController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        IPayFast payFast,
        PayFastCreds payFastCreds,
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
        _payFast = payFast;
        _payFastCreds = payFastCreds;
        _jwtSettings = jwtSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var subscriptions = await _context.Subscriptions
                .AsNoTrackingWithIdentityResolution()
                .Include(s => s.BusinessServicePackage.BusinessService)
                .Include(s => s.BusinessServicePackage.PackageFeatures)
                .Select(s => new
                {
                    s.Id,
                    s.ClientId,
                    s.Client,
                    s.BusinessServicePackageId,
                    BusinessServicePackage = new
                    {
                        s.BusinessServicePackage.Id,
                        s.BusinessServicePackage.Name,
                        s.BusinessServicePackage.Description,
                        s.BusinessServicePackage.Price,
                        s.BusinessServicePackage.BusinessService,
                        s.BusinessServicePackage.PackageFeatures,
                        BillingCycle = EnumHelper.GetDisplayName(s.BusinessServicePackage.BillingCycle),
                        s.BusinessServicePackage.CreatedAt,
                    },
                    s.RecurringCycle,
                    Status = EnumHelper.GetDisplayName(s.Status),
                    s.CreatedAt,
                    s.UpdatedAt
                })
                .ToListAsync();
            return Ok(subscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] Guid Id)
    {
        try
        {
            var subscription = await _context.Subscriptions
                .Where(s => s.Id == Id)
                .Include(s => s.BusinessServicePackage.BusinessService)
                .Include(s => s.BusinessServicePackage.PackageFeatures)
                .Select(s => new
                {
                    s.Id,
                    s.ClientId,
                    s.Client,
                    s.BusinessServicePackageId,
                    BusinessServicePackage = new
                    {
                        s.BusinessServicePackage.Id,
                        s.BusinessServicePackage.Name,
                        s.BusinessServicePackage.Description,
                        s.BusinessServicePackage.Price,
                        s.BusinessServicePackage.BusinessService,
                        s.BusinessServicePackage.PackageFeatures,
                        BillingCycle = EnumHelper.GetDisplayName(s.BusinessServicePackage.BillingCycle),
                        s.BusinessServicePackage.CreatedAt,
                    },
                    s.RecurringCycle,
                    Status = EnumHelper.GetDisplayName(s.Status),
                    s.CreatedAt,
                    s.UpdatedAt
                })
                .FirstOrDefaultAsync();

            return Ok(subscription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
