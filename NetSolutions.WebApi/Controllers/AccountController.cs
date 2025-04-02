using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using NetSolutions.WebApi.Models.Domain;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;

    public AccountController(
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



    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState);

            var user = new Administrator { UserName = model.Username, Email = model.Username };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Send a confirmation email here
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmAccount), "Account", new { userId = user.Id, token }, Request.Scheme);
                await _emailSender.SendEmailAsync(_smtpSettings.Username, user.Email, "Confirm your account", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                return StatusCode(201, user.Id);
            }

            return StatusCode(400, new { message = string.Join(',', result.Errors) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }



    public class LoginModel
    {
        [Required, EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return Unauthorized("Account is locked out.");
                }
                return BadRequest("Invalid login attempt");
            }

            _logger.LogInformation("User logged in.");
            var user = await _userManager.FindByEmailAsync(model.Username) ?? throw new Exception($"User {model.Username} cannot be found.");
            var roles = await _userManager.GetRolesAsync(user);

            var tokensResult = await _jasonWebToken.GenerateTokens(user, roles.ToArray(), Request);
            if (!tokensResult.Succeeded) throw new Exception(string.Join(',', tokensResult.Errors));

            // Return success status 
            var tokensResponse = tokensResult.Response;
            return Ok(tokensResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }




    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var result = await _jasonWebToken.RefreshToken(refreshToken, Request);
            if (!result.Succeeded) return Unauthorized(string.Join(',', result.Errors)); //Return UnAuthorized
            return Ok(result.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPatch("confirm/account/{userId}/{token}")]
    public async Task<IActionResult> ConfirmAccount(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("User ID or Token is missing.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest("Invalid user ID.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok(new { success = true });
        }

        return BadRequest(result.Errors);
    }



    [HttpDelete("{id}"), Authorize(Roles = nameof(Administrator))]
    public async Task<IActionResult> Delete([FromRoute] string Id)
    {
        try
        {
            return NoContent();
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
            throw;
        }
    }


    [HttpGet("userRoles/{userId}")]
    public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"User {userId} cannot be found.");
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles.ToList());
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [HttpGet("user/{Id}"), Authorize]
    public async Task<IActionResult> User([FromRoute] string Id)
    {
        try
        {
            var result = await _context.Users
            .AsNoTrackingWithIdentityResolution()
            .Where(s => s.Id == Id)
            .Include(u => new
            {
                UserRoles = _context.UserRoles
                .Where(ur => ur.UserId == u.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToList()
            })
            .SingleOrDefaultAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
}
