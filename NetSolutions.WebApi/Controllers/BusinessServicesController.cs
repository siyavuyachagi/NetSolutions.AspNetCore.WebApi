using AutoMapper;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Repositories;
using NetSolutions.WebApi.Services;
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
        private readonly ILogger<BusinessServicesController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IJasonWebToken _jasonWebToken;
        private readonly SmtpSettings _smtpSettings;
        private readonly JwtSettings _jwtSettings;
        private readonly IPayFast _payFast;
        private readonly IMapper _mapper;

        public BusinessServicesController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<BusinessServicesController> logger,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IJasonWebToken jasonWebToken,
            SmtpSettings smtpSettings,
            JwtSettings jwtSettings,
            IPayFast payFast,
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
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessServices = await _context.BusinessServices
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Testimonials)
                    .Include(x => x.BusinessServicePackages)
                    .ToListAsync();

                var response = _mapper.Map<List<BusinessServiceDto>>(businessServices);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] Guid Id)
        {
            try
            {
                if (!ModelState.IsValid) return NotFound();

                var businessService = await _context.BusinessServices
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == Id)
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Testimonials)
                    .Include(x => x.BusinessServicePackages)
                    .ToListAsync();

                var response = _mapper.Map<BusinessServiceDto>(businessService);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }



        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid Id)
        {
            try
            {
                await _context.BusinessServices
                    .Where(x => x.Id == Id)
                    .ExecuteDeleteAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
