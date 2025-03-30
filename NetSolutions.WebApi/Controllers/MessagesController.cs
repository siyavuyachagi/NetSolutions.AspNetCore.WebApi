using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetSolutions.Services;
using NetSolutions.Templates.Emails;
using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
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
    private readonly IWebHostEnvironment _webHost;

    public MessagesController(
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
        JwtSettings jwtSettings,
        IWebHostEnvironment webHost)
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
        _webHost = webHost;
    }



    public class ContactUsModel
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(225)]
        public string Subject { get; set; }
        [Required, MaxLength(525)]
        public string Message { get; set; }
    }

    [HttpPost("contact-us")]
    public async Task<IActionResult> ContactUs([FromBody]ContactUsModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var projectCreatedEmail = new ContactUsViewModel(model.Email,model.Subject,model.Message);
            string emailBody = await projectCreatedEmail.HtmlBodyAsync(_webHost);
            var result = await _emailSender.SendEmailAsync(model.Email, _smtpSettings.Email, model.Subject, emailBody);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500,ex.Message);
        }
    }
}
