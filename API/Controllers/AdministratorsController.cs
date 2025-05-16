using Application.Interfaces;
using AutoMapper;
using CloudinaryDotNet;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdministratorsController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJasonWebToken _jasonWebToken;
        private readonly SmtpSettings _smtpSettings;
        private readonly JwtSettings _jwtSettings;
        private readonly Cloudinary _cloudinary;
        private readonly IAdministrator _administratorRepository;

        public AdministratorsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AdministratorsController> logger,
            RoleManager<IdentityRole> roleManager,
            IJasonWebToken jasonWebToken,
            SmtpSettings smtpSettings,
            JwtSettings jwtSettings,
            Cloudinary cloudinary,
            IAdministrator administratorRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _jasonWebToken = jasonWebToken;
            _smtpSettings = smtpSettings;
            _jwtSettings = jwtSettings;
            _cloudinary = cloudinary;
            _administratorRepository = administratorRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var admins = await _administratorRepository.GetAdministratorsAsync();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Details([FromRoute] string Id)
        {
            try
            {
                var admin = await _administratorRepository.GetAdministratorAsync(Id);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }

}

