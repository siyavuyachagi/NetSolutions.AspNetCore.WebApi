using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using System.Security.Cryptography;
using System.Text;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentTransactionsController : ControllerBase
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

    public PaymentTransactionsController(
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
            var transactions = await _context.PaymentTransactions
                .Select(pt => new
                {
                    pt.Id,
                    pt.Status,
                    pt.Amount,
                    pt.AdditionalNotes,
                    pt.CreatedAt,
                })
                .ToListAsync();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] Guid Id)
    {
        try
        {
            var transaction = await _context.PaymentTransactions
                .Where(pt => pt.Id == Id)
                .Select(pt => new
                {
                    pt.Id,
                    pt.Status,
                    pt.Amount,
                    pt.AdditionalNotes,
                    pt.CreatedAt,
                })
                .FirstOrDefaultAsync();
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

}
