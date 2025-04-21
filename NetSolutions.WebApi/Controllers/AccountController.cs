using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using NetSolutions.WebApi.Models.Domain;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using Azure.Core;
using NetSolutions.WebApi.Services;
using NetSolutions.Templates.Emails;
using NetSolutions.WebApi.Templates.Emails;
using Newtonsoft.Json.Linq;
using AutoMapper;
using NetSolutions.WebApi.Repositories;

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
    private readonly IRazorLightEmailRenderer _razorLightEmailRenderer;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;
    private readonly IApplicationUserRepository _applicationUserRepository;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        IRazorLightEmailRenderer razorLightEmailRenderer,
        JwtSettings jwtSettings,
        IMapper mapper,
        IApplicationUserRepository applicationUserRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _jasonWebToken = jasonWebToken;
        _razorLightEmailRenderer = razorLightEmailRenderer;
        _jwtSettings = jwtSettings;
        _mapper = mapper;
        _applicationUserRepository = applicationUserRepository;
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

    /// <summary>
    /// Registers a new user account and sends an email confirmation link.
    /// </summary>
    /// <param name="model">The registration details, including email (used as username) and password.</param>
    /// <param name="returnUrl">
    /// Optional return URL to redirect the user after confirming their account.
    /// Defaults to site root if not provided.
    /// </param>
    /// <returns>
    /// Returns 201 (Created) with the new user info on success,
    /// 400 (Bad Request) with validation or identity errors,
    /// or 500 (Internal Server Error) if registration fails unexpectedly.
    /// </returns>
    [HttpPost("register")]
    [EndpointDescription("Registers a new user and sends a confirmation email.")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model, [FromQuery] string? returnUrl)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Create a new user with default avatar
            var user = new Client
            {
                UserName = model.Username,
                Email = model.Username,
                Avatar = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/avatar.jpg",
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(400, result.Errors.Select(x => x.Description).ToList());
            }

            _logger.LogInformation("User created a new account with password.");

            // Assign Client role
            if (!await _roleManager.RoleExistsAsync(nameof(Client)))
                await _roleManager.CreateAsync(new IdentityRole(nameof(Client)));

            await _userManager.AddToRoleAsync(user, nameof(Client));

            // Send confirmation email
            await SendConfirmationEmail(user.Email, returnUrl);

            return Created(user.Id, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
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

    /// <summary>
    /// Authenticates a user using their credentials and returns access tokens.
    /// </summary>
    /// <param name="model">Login credentials including username, password, and remember-me flag.</param>
    /// <param name="returnUrl"> Return url (optional).</param>
    /// <returns>
    /// Returns 200 (Ok) with JWT access and refresh tokens and user data on success, 
    /// 400 (Bad Request) for invalid login attempts,
    /// 401 (Unauthorized) if the account is locked out, 
    /// 404 (Not Found) if the user does not exist,
    /// or 500 (Internal Server Error) on unexpected errors.
    /// </returns>
    [HttpPost("login")]
    [EndpointDescription("Authenticates a user and returns JWT access and refresh tokens.")]
    public async Task<IActionResult> Login([FromBody] LoginModel model, [FromQuery]string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem();

            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user is null) return NotFound();

            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return Unauthorized("Account is locked out.");

                return BadRequest();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var origin = Request.Headers.Origin;
            if (userRoles.Contains(nameof(Client)) && origin != _jwtSettings.Audiences[0]) return Unauthorized();

            _logger.LogInformation("User logged in.");

            var tokensResult = await _jasonWebToken.GenerateTokens(user.Id, Request);
            if (!tokensResult.Succeeded)
                throw new Exception(string.Join(',', tokensResult.Errors));

            return Ok(tokensResult.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }





    /// <summary>
    /// Refreshes an expired access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token used to obtain a new access token.</param>
    /// <returns>
    /// Returns 200 (OK) with new access and refresh tokens if successful, 
    /// 401 (Unauthorized) if the token is invalid or expired, 
    /// or 500 (Internal Server Error) on unexpected failure.
    /// </returns>
    [HttpGet("refresh/token/{refreshToken}")]
    [EndpointDescription("Exchanges a refresh token for a new access token.")]
    public async Task<IActionResult> RefreshToken([FromRoute] string refreshToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _jasonWebToken.RefreshToken(refreshToken, Request);

            if (!result.Succeeded)
                return Unauthorized(string.Join(',', result.Errors)); // Return 401 if token is invalid or expired

            return Ok(result.Response); // Return the new token set
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="Id">The ID of the user to delete.</param>
    /// <returns>
    /// Returns 204 No Content on successful deletion. 
    /// Returns 500 Internal Server Error if an exception occurs.
    /// </returns>
    [HttpDelete("{Id}"), Authorize]
    [EndpointDescription("Deletes a user by ID. Authorization required.")]
    public async Task<IActionResult> Delete([FromRoute] string Id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddDays(7));

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.Now;

            // Fetch NetSolutions profile for the email template
            var netsolutions = await _context.NetSolutionsProfile
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.PhysicalAddress)
                .Include(x => x.SocialLinks)
            .FirstOrDefaultAsync();

            var accountDeletionSuccessModel = new AccountDeletionSuccess(user, netsolutions, DateTime.Now.AddDays(7));

            // Render email HTML and send
            var htmlBody = await _razorLightEmailRenderer.RenderEmailTemplateAsync(
                nameof(AccountDeletionSuccess),
                accountDeletionSuccessModel
            );

            var emailResult = await _emailSender.SendEmailAsync(
                user.Email,
                "Your account has been deleted.",
                htmlBody
            );

            if (!emailResult.Succeeded)
            {
                _logger.LogError("Error sending Account Registration Email.");
            }

            await _context.SaveChangesAsync();
            return Ok(); // Indicates successful deletion
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message); // Handles any unexpected server errors
        }
    }



    /// <summary>
    /// Sends an account confirmation email to the specified user with a secure token and return URL.
    /// </summary>
    /// <param name="email">The email address of the user to confirm.</param>
    /// <param name="returnUrl">
    /// Optional URL to redirect the user after confirmation. Defaults to "/" if not provided.
    /// </param>
    /// <returns>
    /// Returns 201 (Created) if the email is sent successfully, 404 (NotFound) if the user is not found,
    /// 400 (BadRequest) if the model state is invalid, or 500 (Internal Server Error) if an exception occurs.
    /// </returns>
    [HttpPost("send/confirmation/email/{email}")]
    [EndpointDescription("Sends an account confirmation email with a secure confirmation token.")]
    public async Task<IActionResult> SendConfirmationEmail([FromRoute] string email, [FromQuery] string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound($"User {email} cannot be found.");

            // Generate confirmation token and encode it
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            // Determine return URL
            returnUrl = !string.IsNullOrWhiteSpace(returnUrl)
                ? returnUrl
                : Url.Content("~/");
            returnUrl = System.Web.HttpUtility.UrlEncode(returnUrl);

            // Construct confirmation link
            var confirmationLink = Url.Action(
                nameof(ConfirmAccount),
                "Account",
                new { userId = user.Id, token = encodedToken, returnUrl },
                Request.Scheme
            );

            // Fetch NetSolutions profile for the email template
            var netsolutions = await _context.NetSolutionsProfile
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.PhysicalAddress)
                .Include(x => x.SocialLinks)
                .FirstOrDefaultAsync();

            var accountRegistrationModel = new AccountRegistrationConfirmation(user, netsolutions, confirmationLink, token);

            // Render email HTML and send
            var htmlBody = await _razorLightEmailRenderer.RenderEmailTemplateAsync(
                nameof(AccountRegistrationConfirmation),
                accountRegistrationModel
            );

            var emailResult = await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your account",
                htmlBody
            );

            if (!emailResult.Succeeded)
            {
                _logger.LogError("Error sending Account Registration Email.");
            }

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    /// <summary>
    /// Confirms a user's email address using the provided user ID and confirmation token.
    /// </summary>
    /// <param name="userId">The ID of the user whose email is being confirmed.</param>
    /// <param name="token">The email confirmation token generated earlier.</param>
    /// <param name="returnUrl">Optional redirect URL after successful confirmation.</param>
    /// <returns>
    /// Redirects to the specified frontend route on success. Returns 400 (Bad Request) if the user or token is invalid.
    /// </returns>
    [HttpGet("email/confirmation")]
    [EndpointDescription("Validates an email confirmation token and activates the user's account.")]
    public async Task<IActionResult> ConfirmAccount([FromQuery] string userId, [FromQuery] string token, [FromQuery] string returnUrl)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem("User ID or Token is missing.");

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest("User ID or token is missing.");

            token = System.Web.HttpUtility.UrlDecode(token);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Invalid user ID.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Email confirmation failed for User ID {UserId}. Errors: {Errors}",
                    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            var profileUrl = System.Web.HttpUtility.UrlDecode(returnUrl); // Build profile URL here

            var netsolutions = await _context.NetSolutionsProfile
                //.AsNoTrackingWithIdentityResolution()
                .Include(x => x.PhysicalAddress)
                .Include(x => x.SocialLinks)
                .FirstOrDefaultAsync();

            var accountRegistrationSuccessModel = new AccountRegistrationSuccess(user, netsolutions, profileUrl);

            var htmlBody = await _razorLightEmailRenderer.RenderEmailTemplateAsync(
                nameof(AccountRegistrationSuccess),
                accountRegistrationSuccessModel
            );

            var emailResult = await _emailSender.SendEmailAsync(
                user.Email,
                "Account Registration successful",
                htmlBody
            );

            if (!emailResult.Succeeded)
            {
                _logger.LogError("Error sending Account Registration Success email.");
            }

            returnUrl = string.IsNullOrWhiteSpace(returnUrl)
                ? "https://yourfrontend.dev/account/confirmed" // or some default route
                : System.Web.HttpUtility.UrlDecode(returnUrl);

            return Redirect(returnUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    /// <summary>
    /// Sends a password reset email to the specified user with a secure reset link.
    /// </summary>
    /// <param name="email">The email address of the user requesting password reset.</param>
    /// <param name="returnUrl">
    /// Optional return URL to redirect to after password reset is completed. Defaults to "/auth/reset-password".
    /// </param>
    /// <returns>
    /// Returns 200 (Ok) if the email is sent successfully, 404 (NotFound) if the user does not exist,
    /// 400 (BadRequest) if the model state is invalid, or 500 (Internal Server Error) on unexpected failure.
    /// </returns>
    [HttpPost("forgot-password/{email}")]
    [EndpointDescription("Sends a password reset email to the user with a secure reset link.")]
    public async Task<IActionResult> ForgotPassword([FromRoute] string email, [FromQuery] string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound($"User {email} cannot be found.");

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            returnUrl = !string.IsNullOrWhiteSpace(returnUrl)
                ? returnUrl
                : Url.Content("~/auth/reset-password");

            returnUrl = System.Web.HttpUtility.UrlEncode(returnUrl);

            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { userId = user.Id, token = encodedToken, returnUrl },
                Request.Scheme
            );

            var netsolutions = await _context.NetSolutionsProfile
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.PhysicalAddress)
                .Include(x => x.SocialLinks)
                .FirstOrDefaultAsync();

            var forgotPasswordModel = new PasswordReset(user, netsolutions, resetLink, token);

            var htmlBody = await _razorLightEmailRenderer.RenderEmailTemplateAsync(
                nameof(PasswordReset),
                forgotPasswordModel
            );

            var emailResult = await _emailSender.SendEmailAsync(
                user.Email,
                "Reset Your Password",
                htmlBody
            );

            if (!emailResult.Succeeded)
            {
                _logger.LogError("Error sending Forgot Password email.");
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }



    /// <summary>
    /// Redirects the user to the frontend reset password page with the provided userId and token as query parameters.
    /// </summary>
    /// <param name="userId">The ID of the user requesting the password reset.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="returnUrl">Optional custom return URL (defaults to /auth/reset-password).</param>
    /// <returns>Redirects to the frontend reset password page.</returns>
    [HttpGet("reset-password")]
    [EndpointDescription("Redirects the user to the frontend reset password page with userId and token as query parameters.")]
    public IActionResult ResetPassword(string userId, string token, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Missing user ID or token.");
        }

        returnUrl = System.Web.HttpUtility.UrlDecode(returnUrl);

        var redirectUrl = $"{returnUrl}?userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(token)}";

        return Redirect(redirectUrl);
    }


    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string NewPasswordConfirmation { get; set; }
    }
    /// <summary>
    /// Resets the user's password using the provided reset token and new password.
    /// </summary>
    /// <param name="model">The reset password model containing the user ID, reset token, and new password.</param>
    /// <returns>
    /// Returns 200 (Ok) on success, 400 (BadRequest) on validation failure,
    /// 404 (NotFound) if the user is not found, or 500 (Internal Server Error) on unexpected errors.
    /// </returns>
    [HttpPost("reset-password")]
    [EndpointDescription("Resets the user’s password using the reset token and new password.")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                return NotFound("User not found.");

            model.Token = System.Web.HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok("Password reset successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

}
