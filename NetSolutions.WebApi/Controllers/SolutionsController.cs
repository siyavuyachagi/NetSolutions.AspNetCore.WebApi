using Azure.Core;
using Google.Apis.Discovery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolutionsController : ControllerBase
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
    private readonly IHostEnvironment _hostEnvironment;

    public SolutionsController(
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
        IHostEnvironment hostEnvironment)
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
        _hostEnvironment = hostEnvironment;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var solutions = await _context.Solutions
                .AsNoTrackingWithIdentityResolution()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ProjectId,
                    s.Project,
                    s.Description,
                    s.Price,
                    s.SourceUrl,
                    s.PreviewUrl,
                    s.Version,
                    s.CreatedAt,
                    s.UpdatedAt,
                    Features = s.SolutionFeatures,
                    TechnologyStacks = s.TechnologyStacks.Select(x => x.TechnologyStack).ToList(),
                    Images = s.Images.Select(i => i.FileMetadata).ToList(),
                    Documents = s.Documents.Select(x => x.FileMetadata).ToList(),
                    Reviews = s.Reviews.Select(x => x.Review).ToList(),
                    Discriminator = EF.Property<string>(s, "Discriminator").ToFormattedString(Casing.Pascal).Replace(nameof(Solution), ""),  // Safely handle Discriminator being null
                })
                .ToListAsync();
            return Ok(solutions);
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
            var solutions = await _context.Solutions
                .AsNoTrackingWithIdentityResolution()
                .Include(s => s.TechnologyStacks)
                .ThenInclude(ts => ts.TechnologyStack)
                .Include(s => s.Reviews)
                .ThenInclude(r => r.Review.Evaluator)
                .Where(s => s.Id == Id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ProjectId,
                    s.Project,
                    s.Description,
                    s.Price,
                    s.SourceUrl,
                    s.PreviewUrl,
                    s.Version,
                    s.CreatedAt,
                    s.UpdatedAt,
                    Features = s.SolutionFeatures,
                    Images = s.Images.Select(i => i.FileMetadata).ToList(),
                    Documents = s.Documents.Select(d => d.FileMetadata).ToList(),
                    TechnologyStacks = s.TechnologyStacks.Select(ts => ts.TechnologyStack).ToList(),
                    Reviews = s.Reviews.Select(r => r.Review).ToList(),
                    Likes = s.Likes.Select(l => l.Liker).ToList(),
                    Bookmarks = s.Bookmarks.Select(b => b.Bookmaker).ToList(),
                    Discriminator = EF.Property<string>(s, "Discriminator").ToFormattedString(Casing.Pascal).Replace(nameof(Solution), ""),  // Safely handle Discriminator being null
                })
                .FirstOrDefaultAsync();
            return Ok(solutions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }


    public class PurchaseSolutionModel
    {
        [Required]
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? OrganizationName { get; set; }
    }
    [HttpPost("purchase/{Id}")]
    public async Task<IActionResult> Purchase([FromRoute] Guid Id, PurchaseSolutionModel model)
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
                    PhoneNumber = model.PhoneNumber
                };

                var userResult = await _userManager.CreateAsync(user);

                if (!userResult.Succeeded) throw new Exception(string.Join(", ", userResult.Errors));

                if (!await _roleManager.RoleExistsAsync(nameof(GuestUser)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(GuestUser)));
                await _userManager.AddToRoleAsync(user, nameof(GuestUser));

                if (model.OrganizationName is not null)
                {
                    user.Organization = await _context.Organizations.Where(x => x.Name == model.OrganizationName).FirstOrDefaultAsync()
                        ?? new Organization
                        {
                            Name = model.OrganizationName
                        };
                }
            }

            var solution = await _context.Solutions
                .Where(s => s.Id == Id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Description,
                    s.Price,
                    Discriminator = EF.Property<string>(s, "Discriminator").ToFormattedString(Casing.Pascal)
                })
                .SingleOrDefaultAsync();

            if (solution is null)
                return NotFound($"Solution not found: {Id}");

            string apiBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            // Generate unique payment ID for your records
            string paymentId = Guid.NewGuid().ToString();
            var paymentRequest = new PayFastRequest
            {
                // Payment ID
                PaymentId = paymentId,

                // Transaction details
                Amount = solution.Price.ToString("F2"),
                ItemName = solution.Name,
                ItemDescription = $"Purchase of solution: {solution.Name}",

                // Your site's return URLs
                ReturnUrl = $"{apiBaseUrl}/api/Solutions/purchase/return?paymentId={paymentId}",
                CancelUrl = $"{apiBaseUrl}/api/Solutions/purchase/cancel?paymentId={paymentId}",
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
                TransactionReference = solution.Id.ToString(),
                Status = PaymentTransaction.EStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _context.Add(transaction);


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
            return StatusCode(500, "An error occurred while processing your payment");
        }
    }


    [HttpGet("purchase/return")]
    public async Task<IActionResult> PurchaseReturn([FromQuery] string paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected after successful payment
            // Retrieve the transaction status from your database
            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.Id == Guid.Parse(paymentId));

            if (transaction == null)
            {
                _logger.LogError($"Transaction: {paymentId} cannot be found!");
                return NotFound("Transaction not found");
            }
            transaction.Status = PaymentTransaction.EStatus.Complete;
            await _context.SaveChangesAsync();
            // Redirect to your frontend success page with relevant information
            string returnUrl = $"{Request.Headers.Origin}/solutions/purchase/{transaction.Id}/success)";
            return Redirect(returnUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }


    [HttpGet("purchase/cancel")]
    public async Task<IActionResult> PurchaseCancel([FromQuery] string paymentId)
    {
        try
        {
            // This endpoint is where users will be redirected if they cancel the payment
            // Update the transaction status if needed
            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(t => t.Id == Guid.Parse(paymentId));

            if (transaction is null)
            {
                _logger.LogError($"Error cancelling solution purchase: Transaction: {paymentId} cannot be found!");
                return NotFound($"Transaction: {paymentId} cannot be found!");
            }
            transaction.Status = PaymentTransaction.EStatus.Cancelled;
            await _context.SaveChangesAsync();

            // Redirect to your frontend cancel page
            string clientBaseUrl = _hostEnvironment.IsDevelopment() ? _jwtSettings.Audiences[0] : _jwtSettings.Audiences[1];
            return Redirect($"{clientBaseUrl}/solutions/purchase/{transaction.Id}/cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
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


    [HttpGet("transaction/{Id}")]
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
            throw;
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
    [HttpPost("like/{Id}")]
    public async Task<IActionResult> Like([FromRoute] Guid Id, LikeModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var solution = await _context.Solutions
                .Where(s => s.Id == Id)
                .Include(s => s.Likes)
                .FirstOrDefaultAsync();

            if (solution is null)
            {
                return NotFound($"Solution: {Id} not found!");
            }

            if (solution.Likes.Any(l => l.LikerId == model.UserId))
            {
                var like = solution.Likes
                    .Where(l => l.LikerId == model.UserId)
                    .FirstOrDefault();
                solution.Likes.Remove(like);
            }
            else
            {
                solution.Likes.Add(new Solution_Like
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
            throw;
        }
    }

    public class BookmarkModel
    {
        public string UserId { get; set; }
    }
    [HttpPost("bookmark/{Id}")]
    public async Task<IActionResult> Bookmark([FromRoute] Guid Id, BookmarkModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var solution = await _context.Solutions
                .Where(s => s.Id == Id)
                .Include(s => s.Bookmarks)
                .FirstOrDefaultAsync();

            if (solution is null)
            {
                return NotFound($"Solution: {Id} not found!");
            }

            if (solution.Bookmarks.Any(b => b.BookmakerId == model.UserId))
            {
                var bookmark = solution.Bookmarks
                    .Where(b => b.BookmakerId == model.UserId)
                    .FirstOrDefault();
                solution.Bookmarks.Remove(bookmark);
            }
            else
            {
                solution.Bookmarks.Add(new Solution_Bookmark
                {
                    BookmakerId = model.UserId,
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
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
        [FileExtensions(Extensions =".png, .jpg, .jpeg")]
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
}
