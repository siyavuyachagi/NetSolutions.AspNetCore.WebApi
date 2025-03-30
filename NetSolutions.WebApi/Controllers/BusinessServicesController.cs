using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using static NetSolutions.WebApi.Controllers.BusinessServicesController;

namespace NetSolutions.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessServicesController : ControllerBase
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


        public BusinessServicesController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IJasonWebToken jasonWebToken,
            SmtpSettings smtpSettings,
            JwtSettings jwtSettings,
            IPayFast payFast)
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
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var projects = await _context.BusinessServices
                    .AsNoTracking()
                    .Include(bs => bs.Testimonials)
                    .ThenInclude(t => t.Testimonial.Evaluator)
                    .Include(bs => bs.Packages)
                    .ThenInclude(p => p.PackageFeatures)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Description,
                        s.CreatedAt,
                        s.UpdatedAt,
                        Testimonials = s.Testimonials.Select(t => t.Testimonial),
                        Thumbnail = s.Thumbnail.FileMetadata.ViewLink,
                        Packages = s.Packages.Select(p => new
                        {
                            p.Id,
                            p.Name,
                            p.Price,
                            BillingCycle = EnumHelper.GetDisplayName(p.BillingCycle),
                            p.Description,
                            p.BusinessServiceId,
                            p.PackageFeatures,
                            p.CreatedAt,
                        }),
                    })
                    .ToListAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute]Guid Id)
        {
            try
            {
                var projects = await _context.BusinessServices
                    .AsNoTracking()
                    .Where(s => s.Id == Id)
                    .Include(bs => bs.Testimonials)
                    .ThenInclude(t => t.Testimonial.Evaluator)
                    .Include(bs => bs.Packages)
                    .ThenInclude(p => p.PackageFeatures)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Description,
                        s.CreatedAt,
                        s.UpdatedAt,
                        Testimonials = s.Testimonials.Select(t => t.Testimonial),
                        Thumbnail = s.Thumbnail.FileMetadata.ViewLink,
                        Packages = s.Packages.Select(p => new
                        {
                            p.Id,
                            p.Name,
                            p.Price,
                            BillingCycle = EnumHelper.GetDisplayName(p.BillingCycle),
                            p.Description,
                            p.BusinessServiceId,
                            p.PackageFeatures,
                            p.CreatedAt,
                        }),
                    })
                    .FirstOrDefaultAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
