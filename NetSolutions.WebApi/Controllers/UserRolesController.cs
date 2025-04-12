using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
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
        private readonly Cloudinary _cloudinary;

        public UserRolesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<AccountController> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IJasonWebToken jasonWebToken, SmtpSettings smtpSettings, JwtSettings jwtSettings, Cloudinary cloudinary)
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
            _cloudinary = cloudinary;
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] string Id)
        {
            try
            {
                var userRole = await _context.Roles
                    .Where(r => r.Id == Id)
                    .ToListAsync();
                return Ok(userRole);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpGet("user/{Id}")]
        public async Task<IActionResult> UserRoles([FromRoute] string Id)
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == Id)
                    .Join(_context.Roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new {
                            role.Id,
                            role.Name,
                            role.NormalizedName,
                            // Include other role properties as needed
                        })
                    .ToListAsync();
                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
