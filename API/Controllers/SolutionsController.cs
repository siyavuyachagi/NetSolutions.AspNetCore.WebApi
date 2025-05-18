using Application.DTOs.Request;
using Application.Helpers;
using Application.Interfaces;
using Application.Templates.Emails;
using CloudinaryDotNet;
using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Web;
using static Domain.Entities.FileMetadata;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolutionsController : ControllerBase
{
    private readonly ILogger<SolutionsController> _logger;
    private readonly ISolution _solutionRepository;
    private readonly IApplicationUser _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOrganization _organization;
    private readonly IPayFast _payFast;
    private readonly IPaymentTransaction _paymentTransaction;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly JwtSettings _jwtSettings;
    private readonly IRazorLightEmailRenderer _razorLightEmailRenderer;
    private readonly IEmailSender _emailSender;
    private readonly IBusinessProfile _businessProfile;
    private readonly GithubConfig _githubConfig;


    public SolutionsController(
        ILogger<SolutionsController> logger,
        ISolution solutionRepository,
        IApplicationUser applicationUserRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOrganization organization,
        IPayFast payFast,
        IPaymentTransaction paymentTransaction,
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment,
        JwtSettings jwtSettings,
        IRazorLightEmailRenderer razorLightEmailRenderer,
        IEmailSender emailSender,
        IBusinessProfile businessProfile,
        GithubConfig githubConfig)
    {
        _logger = logger;
        _solutionRepository = solutionRepository;
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _organization = organization;
        _payFast = payFast;
        _paymentTransaction = paymentTransaction;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _jwtSettings = jwtSettings;
        _razorLightEmailRenderer = razorLightEmailRenderer;
        _emailSender = emailSender;
        _businessProfile = businessProfile;
        _githubConfig = githubConfig;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _solutionRepository.GetSolutionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] Guid Id)
    {
        try
        {
            var result = await _solutionRepository.GetSolutionAsync(Id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    public class PurchaseSolutionRequest
    {
        [Required]
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? OrganizationName { get; set; }
    }
    [HttpPost("{id}/purchase")]
    public async Task<IActionResult> Purchase(
        [FromRoute] Guid id, 
        [FromBody] PurchaseSolutionRequest model, 
        [FromQuery] string successRedirectUrl, 
        [FromQuery] string cancelRedirectUrl)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // User identification
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                user = new GuestUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                };

                if (model.OrganizationName is not null)
                {
                    var organization = await _organization.GetOrganizationByNameAsync(model.OrganizationName);
                    if (organization is not null) user.OrganizationId = organization.Id;
                    else
                    {
                        var newOrganization = new Organization
                        {
                            Id = Guid.NewGuid(),
                            Name = model.OrganizationName,
                        };
                        await _organization.CreateOrganizationAsync(newOrganization);
                        user.OrganizationId = newOrganization.Id;
                    }
                }

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description).ToList()));

                if (!await _roleManager.RoleExistsAsync(nameof(GuestUser)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(GuestUser)));
                await _userManager.AddToRoleAsync(user, nameof(GuestUser));

            }

            var solution = await _solutionRepository.GetSolutionAsync(id);

            if (solution is null)
                return NotFound($"Solution not found: {id}");

            string apiBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            // Generate unique payment ID for your records
            string paymentId = Guid.NewGuid().ToString();
            // Format return url

            string rawSuccessUrl = $"{successRedirectUrl}?uid={user.Id}&pid={paymentId}&sid={solution.Id}";
            string rawCancelUrl = $"{cancelRedirectUrl}?uid={user.Id}&pid={paymentId}&sid={solution.Id}";

            var paymentRequest = new PayFastRequest
            {
                // Payment ID
                PaymentId = paymentId,

                // Transaction details
                Amount = solution.Price.ToString("F2"),
                ItemName = solution.Name,
                ItemDescription = $"Purchase of solution: {solution.Name}",

                // Your site's return URLs
                ReturnUrl = $"{apiBaseUrl}/api/Solutions/purchase/return?redirectUrl={HttpUtility.UrlEncode(rawSuccessUrl)}",
                CancelUrl = $"{apiBaseUrl}/api/Solutions/purchase/cancel?redirectUrl={HttpUtility.UrlEncode(rawCancelUrl)}",
                NotifyUrl = $"{apiBaseUrl}/api/Solutions/purchase/notify",

                // Buyer details
                EmailAddress = user.Email ?? user.UserName ?? model.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,

                // Optional: Store reference to identify transaction in your system
                CustomStr1 = solution.Id.ToString(),
                CustomStr2 = user.Id,  // Assuming the model contains UserId
            };

            var initResult = await _payFast.InitAsync(paymentRequest, IPayFast.Environment.Sandbox);
            if (!initResult.Succeeded) return StatusCode(500, string.Join(", ", initResult.Errors));

            // Construct the PayFast redirect URL
            var payFastUrl = $"{initResult.Response}";

            // Create a record of this payment attempt in your database
            var transaction = new PaymentTransaction
            {
                Id = Guid.Parse(paymentRequest.PaymentId),
                Amount = Decimal.Parse(paymentRequest.Amount),
                PaymentMethod = "cc",
                TransactionReference = solution.Id.ToString(),
                Status = PaymentTransaction.EStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await _paymentTransaction.CreatePaymentTransactionAsync(transaction);


            //Junction table
            var solutionTransaction = new User_Solution_PaymentTransaction
            {
                UserId = user.Id,
                SolutionId = solution.Id,
                PaymentTransactionId = transaction.Id,
            };
            _context.Add(solutionTransaction);

            await _context.SaveChangesAsync();
            return Ok(new { redirectUrl = payFastUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "An error occurred while processing your payment");
        }
    }


    public class PurchaseReturnQuery
    {
        [Required]
        public string RedirectUrl { get; set; }
    }
    [HttpGet("purchase/return")]
    public async Task<IActionResult> PurchaseReturn([FromQuery] PurchaseReturnQuery model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var uri = new Uri(model.RedirectUrl);
            var queryCollection = HttpUtility.ParseQueryString(uri.Query);
            var userId = queryCollection["uid"];
            var paymentId = queryCollection["pid"];
            var solutionId = queryCollection["sid"];

            if(string.IsNullOrWhiteSpace(userId)
                || string.IsNullOrWhiteSpace(paymentId)
                || string.IsNullOrWhiteSpace(solutionId))
            {
                return BadRequest("One or more parameters are required.");
            }

            // Retrieve the transaction status from your database
            var transaction = await _paymentTransaction.GetPaymentTransactionAsync(Guid.Parse(paymentId));
            if (transaction == null)
            {
                _logger.LogError($"Transaction: {paymentId} cannot be found!");
                return NotFound("Transaction not found");
            }
            await _paymentTransaction.UpdateStatusAsync(transaction.Id, PaymentTransaction.EStatus.Complete);

            // Retrieve solution and mark as sold
            var solution = await _solutionRepository.GetSolutionAsync(Guid.Parse(solutionId));
            if (solution == null)
            {
                _logger.LogError($"Solution: {solutionId} cannot be found!");
                return NotFound("Solution not found");
            }

            try
            {
                // Send notification
                var applicationUser = await _applicationUserRepository.GetApplicationUserAsync(userId);
                var businessProfile = await _businessProfile.GetBusinessProfileAsync();
                var solutionPurchaseSuccess = new SolutionPurchaseSuccess(applicationUser, businessProfile, DateTime.Now.AddDays(7));
                // Render email HTML and send
                var htmlBody = await _razorLightEmailRenderer.RenderEmailTemplateAsync(
                    nameof(SolutionPurchaseSuccess),
                    solutionPurchaseSuccess
);
                var emailResult = await _emailSender.SendEmailAsync(
                    applicationUser.Email,
                    "Your account has been deleted.",
                    htmlBody
                );

                if (!emailResult.Succeeded)
                {
                    _logger.LogError("Error sending Account Registration Email.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            // Redirect to your frontend success page with relevant information
            return Redirect(model.RedirectUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("purchase/cancel")]
    public async Task<IActionResult> PurchaseCancel([FromQuery] string redirectUrl)
    {
        try
        {
            var uri = new Uri(redirectUrl);
            var queryCollection = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var paymentId = queryCollection["pid"];

            if (string.IsNullOrWhiteSpace(paymentId))
            {
                return BadRequest("Invalid query parameters.");
            }

            var transaction = await _paymentTransaction.GetPaymentTransactionAsync(Guid.Parse(paymentId));
            if (transaction == null)
            {
                _logger.LogError($"Transaction: {paymentId} cannot be found!");
                return NotFound("Transaction not found");
            }
            await _paymentTransaction.UpdateStatusAsync(transaction.Id, PaymentTransaction.EStatus.Cancelled);

            // Redirect to your frontend cancel page
            //string clientBaseUrl = _webHostEnvironment.IsDevelopment() ? _jwtSettings.Audiences[0] : _jwtSettings.Audiences[1];
            string clientBaseUrl = string.Empty;
            return Redirect($"{clientBaseUrl}/solutions/purchase/{transaction.Id}/cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost("purchase/notify")]
    public async Task<IActionResult> PurchaseNotify()
    {
        try
        {
            // Read the form data from the request
            var form = await Request.ReadFormAsync();

            // Convert form to dictionary for easier access
            var formData = form.ToDictionary(x => x.Key, x => x.Value.ToString());

            // Validate that the request is from PayFast
            if (!await _payFast.ValidatePaymentNotificationAsync(formData, HttpContext))
            {
                return BadRequest("Invalid notification");
            }

            // Extract payment information
            string paymentId = formData["m_payment_id"];
            string paymentStatus = formData["payment_status"];
            string paymentMethod = formData["payment_method"];
            decimal amount = decimal.Parse(formData["amount_gross"]);

            // Find the transaction in your database
            var solutionTransaction = await _context.User_Solution_PaymentTransactions.FindAsync(Guid.Parse(paymentId));

            if (solutionTransaction == null) return NotFound("Transaction not found");

            // Update transaction status based on PayFast status
            switch (paymentStatus.ToLower())
            {
                case "complete":
                    solutionTransaction.PaymentTransaction.Status = PaymentTransaction.EStatus.Successfull;
                    // Add your business logic for successful payment
                    // For example, grant access to the purchased solution
                    await GrantAccessToSolution(solutionTransaction.UserId, solutionTransaction.SolutionId);
                    break;

                case "failed":
                    solutionTransaction.PaymentTransaction.Status = PaymentTransaction.EStatus.Failed;
                    break;

                case "pending":
                    solutionTransaction.PaymentTransaction.Status = PaymentTransaction.EStatus.Pending;
                    break;

                case "cancelled":
                    solutionTransaction.PaymentTransaction.Status = PaymentTransaction.EStatus.Cancelled;
                    break;
            }

            // Update transaction with additional details
            solutionTransaction.PaymentTransaction.TransactionReference = formData["pf_payment_id"];
            solutionTransaction.PaymentTransaction.UpdatedAt = DateTime.UtcNow;
            solutionTransaction.PaymentTransaction.AdditionalNotes = $"PayFast status: {paymentStatus}";

            // Save changes to database
            await _context.SaveChangesAsync();

            // PayFast expects a 200 OK response with empty content
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // Always return OK to PayFast even if we have internal errors
            // This prevents PayFast from retrying the notification
            // But we should log the error for investigation
            return Ok();
        }
    }


    [HttpGet("{Id}/transaction")]
    public async Task<IActionResult> SolutionTransactionDetails([FromRoute] Guid Id)
    {
        try
        {
            var transaction = await _context.User_Solution_PaymentTransactions
                .Where(x => x.PaymentTransaction.Id == Id)
                .Select(pt => pt.PaymentTransaction)
                .FirstOrDefaultAsync();
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    private async Task GrantAccessToSolution(string userId, Guid solutionId)
    {
        try
        {
            // Implement your logic to grant the user access to the purchased solution
            // For example, create a record in UserSolutions table
            int affectedRows = _context.Solutions
                .Where(x => x.Id == solutionId)
                .ExecuteUpdate(setters => setters.SetProperty(x => x.IsDeleted, true));

            if (affectedRows == 0)
                _logger.LogError($"Purchased solution: {solutionId} not updated");

            var userSolution = new User_Solution
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SolutionId = solutionId,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Add(userSolution);
            await _context.SaveChangesAsync();

            // You might also want to send an email notification to the user
            // await _emailService.SendPurchaseConfirmationEmail(userId, solutionId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error granting User to Solution, {ex.Message}");
            throw;
        }
    }


    public class LikeModel
    {
        [Required]
        public string UserId { get; set; }
    }
    [HttpPost("{Id}/like")]
    public async Task<IActionResult> Like([FromRoute] Guid Id, LikeModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var solution = await _context.Solutions
                .Where(s => s.Id == Id)
                .Include(s => s.Solution_Likes)
                .FirstOrDefaultAsync();

            if (solution is null)
            {
                return NotFound($"Solution: {Id} not found!");
            }

            if (solution.Solution_Likes.Any(l => l.LikerId == model.UserId))
            {
                var like = solution.Solution_Likes
                    .Where(l => l.LikerId == model.UserId)
                    .FirstOrDefault();
                solution.Solution_Likes.Remove(like);
            }
            else
            {
                solution.Solution_Likes.Add(new Solution_Like
                {
                    LikerId = model.UserId,
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    public class CreateSolutionModel
    {
        // Basic Info
        public Guid? ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        // Technical Info
        public string? SourceUrl { get; set; }
        public string? PreviewUrl { get; set; }
        public List<Guid>? TechnologyStackIds { get; set; }
        //Files & Documentation
        [FileExtensions(Extensions = ".png, .jpg, .jpeg")]
        public List<IFormFile>? DisplayImages { get; set; }
        public List<IFormFile>? DocumentationFiles { get; set; }
        public string? AdditionalNotes { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateSolutionModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);


            var solution = new Solution
            {
                Name = model.Name,
                Description = model.Description,
                Version = model.Version,
                //SourceUrl = model.SourceUrl,
                //PreviewUrl = model.PreviewUrl,
            };

            return Created(string.Empty, solution.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid Id)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadSolution([FromRoute] Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _solutionRepository.DownloadSolutionZipAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var file = result.Response;

            return File(file.Content, file.ContentType, file.FileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

}
