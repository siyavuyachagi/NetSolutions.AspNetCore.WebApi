using Application.DTOs.Request;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusinessServicePackagesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<BusinessServicePackagesController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly JwtSettings _jwtSettings;
    private readonly IPayFast _payFast;
    private readonly PayFastConfig _payFastCreds;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;
    private readonly IBusinessServicePackage _businessServicePackage;
    private readonly IOrganization _organization;
    private readonly ISubscription _subscription;
    private readonly IPaymentTransaction _paymentTransaction;

    public BusinessServicePackagesController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<BusinessServicePackagesController> logger,
        RoleManager<IdentityRole> roleManager,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        JwtSettings jwtSettings,
        IPayFast payFast,
        PayFastConfig payFastCreds,
        IHostEnvironment hostEnvironment,
        IMapper mapper,
        IBusinessServicePackage businessServicePackage,
        IOrganization organization,
        ISubscription subscription,
        IPaymentTransaction paymentTransaction)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
        _jwtSettings = jwtSettings;
        _payFast = payFast;
        _payFastCreds = payFastCreds;
        _hostEnvironment = hostEnvironment;
        _mapper = mapper;
        _businessServicePackage = businessServicePackage;
        _organization = organization;
        _subscription = subscription;
        _paymentTransaction = paymentTransaction;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var businessServicesPackages = await _businessServicePackage.GetBusinessServicePackagesAsync();
            return Ok(businessServicesPackages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        try
        {
            var businessServicesPackage = await _businessServicePackage.GetBusinessServicePackageAsync(id);
            return Ok(businessServicesPackage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
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

    [HttpPost("{Id}/subscribe")]
    public async Task<IActionResult> Subscribe([FromRoute] Guid Id, [FromBody] SubscibeModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            //  Get service package
            var businessServicePackage = await _businessServicePackage.GetBusinessServicePackageAsync(Id);
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
                    await _organization.GetOrganizationByNameAsync(model.OrganizationName);
                    var organization = await _organization.GetOrganizationAsync(Id);
                    if(organization is not null)
                    {
                        user.OrganizationId = organization.Id;
                    }
                    else
                    {
                        await _organization.CreateOrganizationAsync(new Organization
                        {
                            Name = model.OrganizationName,
                        });
                    }
                }
            }

            // Create subscription
            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                ClientId = user.Id,
                BusinessServicePackageId = businessServicePackage.Id,
                Status = Subscription.EStatus.Active,
                CreatedAt = DateTime.Now,
            };
            await _subscription.CreateSubscriptionAsync(subscription);



            // Save transaction records
            var transaction = new PaymentTransaction
            {
                Amount = subscription.BusinessServicePackage.Price,
                Status = PaymentTransaction.EStatus.Pending,
                TransactionReference = subscription.Id.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            await _paymentTransaction.CreatePaymentTransactionAsync(transaction);

            // PayFast subscription request

            //string returnUrl = Request.HttpContext.
            string apiBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var request = new PayFastSubscriptionRequest
            {
                // Payment ID
                PaymentId = transaction.Id,

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

            return Ok(new { redirectUrl = payFastResult.Response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("subscribe/return")]
    public async Task<IActionResult> SubscribeReturn([FromQuery] Guid subscriptionId, [FromQuery] Guid paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected after successful payment
            if (!ModelState.IsValid) return ValidationProblem(ModelState);


            var subscription = await _subscription.GetSubscriptionAsync(subscriptionId);
            var paymentTransaction = await _paymentTransaction.GetPaymentTransactionAsync(paymentId);

            if (subscription is null || paymentTransaction is null)
            {
                string errorMessage = "Error proccessing request: ";
                if (subscription is null) errorMessage += $"Subscription Id: {subscriptionId} cannot be found!";
                else if (paymentTransaction is null) errorMessage += $"Payment Id: {paymentId} cannot be found!";
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            await _subscription.UpdateStatusAsync(subscription.Id, Subscription.EStatus.Active);
            await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Successfull);

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
    public async Task<IActionResult> PurchaseCancel([FromQuery] Guid subscriptionId, [FromQuery] Guid paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected after successful payment
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var subscription = await _subscription.GetSubscriptionAsync(subscriptionId);
            var paymentTransaction = await _paymentTransaction.GetPaymentTransactionAsync(paymentId);

            if (subscription is null || paymentTransaction is null)
            {
                string errorMessage = "Error proccessing request: ";
                if (subscription is null) errorMessage += $"Subscription Id: {subscriptionId} cannot be found!";
                else if (paymentTransaction is null) errorMessage += $"Payment Id: {paymentId} cannot be found!";
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            await _subscription.UpdateStatusAsync(subscription.Id, Subscription.EStatus.Cancelled);
            await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Cancelled);

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
            var paymentTransaction = await _paymentTransaction.GetPaymentTransactionAsync(Guid.Parse(paymentId));


            if (paymentTransaction == null) return NotFound("Transaction not found");

            // Update transaction status based on PayFast status
            switch (paymentStatus.ToLower())
            {
                case "complete":
                    await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Successfull);
                    break;

                case "failed":
                    await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Failed);
                    break;

                case "pending":
                    await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Pending);
                    break;

                case "cancelled":
                    await _paymentTransaction.UpdateStatusAsync(paymentTransaction.Id, PaymentTransaction.EStatus.Cancelled);
                    break;
            }

            // Update transaction with additional details
            await _paymentTransaction.UpdatePaymentTransactionAsync(id: paymentTransaction.Id, transactionReference: formData["pf_payment_id"], additionalNotes: $"PayFast status: {paymentStatus}");

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
                var paymentTransaction = await _paymentTransaction.GetPaymentTransactionAsync(Guid.Parse(paymentId));

                if (paymentTransaction != null && formData.ContainsKey("amount_gross"))
                {
                    decimal notifiedAmount = decimal.Parse(formData["amount_gross"]);
                    if (paymentTransaction.Amount != notifiedAmount)
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
