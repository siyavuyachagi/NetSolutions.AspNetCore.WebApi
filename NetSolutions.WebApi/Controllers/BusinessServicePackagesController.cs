using AutoMapper;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NetSolutions.Helpers;
using NetSolutions.Models.Enums;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusinessServicePackagesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly JwtSettings _jwtSettings;
    private readonly IPayFast _payFast;
    private readonly PayFastCreds _payFastCreds;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public BusinessServicePackagesController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        JwtSettings jwtSettings,
        IPayFast payFast,
        PayFastCreds payFastCreds,
        IHostEnvironment hostEnvironment,
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
        _jwtSettings = jwtSettings;
        _payFast = payFast;
        _payFastCreds = payFastCreds;
        _hostEnvironment = hostEnvironment;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var businessServicesPackages = await _context.BusinessServicePackages
                .Include(x => x.BusinessService)
                .Include(x => x.BusinessServicePackageFeatures)
                .Include(x => x.Subscriptions)
                .ToListAsync();

            var businessServicePackagesDto = _mapper.Map<List<BusinessServicePackageDto>>(businessServicesPackages);
            return Ok(businessServicePackagesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] Guid Id)
    {
        try
        {
            var businessServicesPackage = await _context.BusinessServicePackages
                .Where(x => x.Id == Id)
                .Include(x => x.BusinessService)
                .Include(x => x.BusinessServicePackageFeatures)
                .Include(x => x.Subscriptions)
                .ToListAsync();

            var businessServicePackageDto = _mapper.Map<BusinessServicePackageDto>(businessServicesPackage);
            return Ok(businessServicePackageDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public class SubscibeModel
    {
        // Project Details
        public string Name { get; set; }
        public string Description { get; set; }
        public string? TargetAudience { get; set; }
        public List<DateTime> Timeline { get; set; }
        public string? AdditionalNotes { get; set; }

        // Attachments
        //public List<object>? Files { get; set; }

        // Contact Info
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string? OrganizationName { get; set; }
    }

    [HttpPost("subscribe/{Id}")]
    public async Task<IActionResult> Subscribe([FromRoute] Guid Id, [FromBody] SubscibeModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            //  Get service package
            var businessServicePackage = await _context.BusinessServicePackages
                .Where(sp => sp.Id == Id)
                .FirstOrDefaultAsync();
            if (businessServicePackage is null) throw new Exception($"Service package Id:{Id} not found!");

            // Create user prorfile
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                user = new Client
                {
                    UserName = model.Email,
                    Email = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    PhoneNumber = model.PhoneNumber
                };

                var userResult = await _userManager.CreateAsync(user);

                if (!userResult.Succeeded) throw new Exception(string.Join(", ", userResult.Errors));

                if (!await _roleManager.RoleExistsAsync(nameof(Client)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(Client)));
                await _userManager.AddToRoleAsync(user, nameof(Client));

                if (model.OrganizationName is not null)
                {
                    user.Organization = await _context.Organizations.Where(x => x.Name == model.OrganizationName).FirstOrDefaultAsync()
                        ?? new Organization
                        {
                            Name = model.OrganizationName
                        };
                }
            }

            // Create subscription
            var subscription = new Subscription
            {
                ClientId = user.Id,
                BusinessServicePackageId = businessServicePackage.Id,
                Status = Subscription.EStatus.Active,
                CreatedAt = DateTime.Now,
            };
            _context.Subscriptions.Add(subscription);

            // Save transaction records
            var transaction = new PaymentTransaction
            {
                Amount = subscription.BusinessServicePackage.Price,
                Status = PaymentTransaction.EStatus.Pending,
                TransactionReference = subscription.Id.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            _context.PaymentTransactions.Add(transaction);

            // PayFast subscription request
            int frequency = 3; // Default to Monthly (PayFast uses 3 for Monthly)
            switch (businessServicePackage.BillingCycle)
            {
                case BillingCycle.None:
                    frequency = 0; // No subscription
                    break;
                case BillingCycle.FourthNight:
                    frequency = 1; // Closest option in PayFast is daily (1)
                    break;
                case BillingCycle.Weekly:
                    frequency = 2; // PayFast uses 2 for Weekly
                    break;
                case BillingCycle.Monthly:
                    frequency = 3; // PayFast uses 3 for Monthly
                    break;
                case BillingCycle.Quarterly:
                    frequency = 4; // PayFast uses 4 for Quarterly
                    break;
                case BillingCycle.Yearly:
                    frequency = 6; // PayFast uses 6 for Annually
                    break;
                default:
                    frequency = 0; // Default to no subscription
                    break;
            }

            //string returnUrl = Request.HttpContext.
            string apiBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var request = new PayFastSubscriptionRequest
            {
                // Payment ID
                PaymentId = transaction.Id,
                Frequency = frequency,

                // Transaction details
                RecurringAmount = Math.Round(businessServicePackage.Price, 2),
                ItemName = businessServicePackage.Name,
                ItemDescription = $"{businessServicePackage.Name} subscription",

                // Your site's return URLs
                ReturnUrl = $"{apiBaseUrl}/api/BusinessServicePackages/subscribe/return?subscriptionId={subscription.Id}&paymentId={transaction.Id}",
                CancelUrl = $"{apiBaseUrl}/api/BusinessServicePackages/cancel?subscriptionId={subscription.Id}&paymentId={transaction.Id}",
                NotifyUrl = $"{apiBaseUrl}/api/BusinessServicePackages/notify?subscriptionId={subscription.Id}",

                // Buyer details
                EmailAddress = user.Email ?? user.UserName ?? model.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                CellNumber = user.PhoneNumber,

                // Optional: Store reference to identify transaction in your system
                CustomStr1 = businessServicePackage.Id.ToString(),
                CustomStr2 = user.Id,  // Assuming the model contains UserId
            };

            var payFastResult = await _payFast.SubscribeAsync(request);
            if (!payFastResult.Succeeded) throw new Exception(string.Join(", ", payFastResult.Errors));

            await _context.SaveChangesAsync();
            return Ok(new { redirectUrl = payFastResult.Response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("subscribe/return")]
    public async Task<IActionResult> SubscribeReturn([FromQuery] Guid SubscriptionId, [FromQuery] Guid paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected after successful payment
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var subscription = await _context.Subscriptions
                .Where(s => s.Id == SubscriptionId)
                .FirstOrDefaultAsync();

            var paymentTransaction = await _context.PaymentTransactions
                .Where(pt => pt.Id == paymentId)
                .FirstOrDefaultAsync();

            if (subscription is null || paymentTransaction is null)
            {
                string errorMessage = "Error proccessing request: ";
                if (subscription is null) errorMessage += $"Subscription Id: {SubscriptionId} cannot be found!";
                else if (paymentTransaction is null) errorMessage += $"Payment Id: {paymentId} cannot be found!";
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            subscription.Status = Subscription.EStatus.Active;
            paymentTransaction.Status = PaymentTransaction.EStatus.Successfull;

            await _context.SaveChangesAsync();
            // Redirect to your frontend success page with relevant information
            string clientBaseUrl = _hostEnvironment.IsDevelopment() ? _jwtSettings.Audiences[0] : _jwtSettings.Audiences[1];
            return Redirect($"{clientBaseUrl}/services/getting-started/success?subscriptionId={subscription.Id}&paymentId={paymentTransaction.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    [HttpGet("subscribe/cancel")]
    public async Task<IActionResult> PurchaseCancel([FromQuery] Guid SubscriptionId, [FromQuery] Guid paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected after successful payment
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var subscription = await _context.Subscriptions
                .Include(s => s.BusinessServicePackage)
                .Where(s => s.Id == SubscriptionId)
                .FirstOrDefaultAsync();

            var paymentTransaction = await _context.PaymentTransactions
                .Where(pt => pt.Id == paymentId)
                .FirstOrDefaultAsync();

            if (subscription is null || paymentTransaction is null)
            {
                string errorMessage = "Error proccessing request: ";
                if (subscription is null) errorMessage += $"Subscription Id: {SubscriptionId} cannot be found!";
                else if (paymentTransaction is null) errorMessage += $"Payment Id: {paymentId} cannot be found!";
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            subscription.Status = Subscription.EStatus.Cancelled;
            paymentTransaction.Status = PaymentTransaction.EStatus.Cancelled;

            await _context.SaveChangesAsync();
            // Redirect to your frontend success page with relevant information
            string clientBaseUrl = _hostEnvironment.IsDevelopment() ? _jwtSettings.Audiences[0] : _jwtSettings.Audiences[1];
            return Redirect($"{clientBaseUrl}/services/getting-started/cancelled?subscriptionId={subscription.Id}&paymentId={paymentTransaction.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    [HttpPost("subscribe/notify")]
    public async Task<IActionResult> PurchaseNotify()
    {
        try
        {
            // Read the form data from the request
            var form = await Request.ReadFormAsync();

            // Convert form to dictionary for easier access
            var formData = form.ToDictionary(x => x.Key, x => x.Value.ToString());

            // Validate that the request is from PayFast
            if (!await ValidatePayFastNotification(formData))
            {
                return BadRequest("Invalid notification");
            }

            // Extract payment information
            string paymentId = formData["m_payment_id"];
            string paymentStatus = formData["payment_status"];
            string paymentMethod = formData["payment_method"];
            decimal amount = decimal.Parse(formData["amount_gross"]);

            // Find the transaction in your database
            var solutionTransaction = await _context.User_Solution_PaymentTransactions
                .FirstOrDefaultAsync(t => t.PaymentTransactionId == Guid.Parse(paymentId));

            if (solutionTransaction == null) return NotFound("Transaction not found");

            // Update transaction status based on PayFast status
            switch (paymentStatus.ToLower())
            {
                case "complete":
                    solutionTransaction.PaymentTransaction.Status = PaymentTransaction.EStatus.Successfull;
                    // Add your business logic for successful payment
                    // For example, grant access to the purchased solution
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

    private async Task<bool> ValidatePayFastNotification(Dictionary<string, string> formData)
    {
        try
        {
            // 1. Check if the notification is from the correct environment
            bool isSandbox = true;
            string expectedHost = isSandbox ? "sandbox.payfast.co.za" : "www.payfast.co.za";

            // Get client IP
            string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            // In production, validate that the request is coming from PayFast's IP range
            // For sandbox testing, you might skip this check
            if (!isSandbox)
            {
                // Simple example - in production you'd check against PayFast's IP ranges
                // This is a placeholder for the actual IP validation
                var payFastIps = new List<string> { "204.93.162.1", "204.93.163.1" };
                if (!payFastIps.Contains(clientIp))
                {
                    return false;
                }
            }

            // 2. Verify the signature
            string receivedSignature = formData.ContainsKey("signature") ? formData["signature"] : "";

            // Create a dictionary without the signature for validation
            var dataForSignature = formData
                .Where(kvp => kvp.Key != "signature")
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Calculate the expected signature
            string calculatedSignature = CalculateSignature(dataForSignature);

            if (receivedSignature != calculatedSignature)
            {
                return false;
            }

            // 3. Verify the payment data (optional but recommended)
            // Check if the amount matches your records
            if (formData.ContainsKey("m_payment_id"))
            {
                string paymentId = formData["m_payment_id"];
                var transaction = await _context.PaymentTransactions
                    .FirstOrDefaultAsync(t => t.Id == Guid.Parse(paymentId));

                if (transaction != null && formData.ContainsKey("amount_gross"))
                {
                    decimal notifiedAmount = decimal.Parse(formData["amount_gross"]);
                    if (transaction.Amount != notifiedAmount)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    private string CalculateSignature(Dictionary<string, string> data)
    {
        try
        {
            // Order parameters alphabetically
            var orderedData = data.OrderBy(d => d.Key);

            // Create parameter string
            var parameterString = string.Join("&",
                orderedData.Select(d => $"{d.Key}={Uri.EscapeDataString(d.Value)}"));

            // Add passphrase if configured
            var passphrase = _payFastCreds.PassPhrase;
            if (!string.IsNullOrEmpty(passphrase))
            {
                parameterString += $"&passphrase={Uri.EscapeDataString(passphrase)}";
            }

            // Generate MD5 hash
            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(parameterString));

            // Convert to hex string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
