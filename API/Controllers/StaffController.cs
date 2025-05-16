using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<StaffController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJasonWebToken _jasonWebToken;
    private readonly SmtpSettings _smtpSettings;
    private readonly IMapper _mapper;
    private readonly IStaff _staffRepository;
    private readonly ApplicationDbContext _context;

    public StaffController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<StaffController> logger,
        RoleManager<IdentityRole> roleManager,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        IMapper mapper,
        IStaff staffRepository,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
        _jasonWebToken = jasonWebToken;
        _smtpSettings = smtpSettings;
        _mapper = mapper;
        _staffRepository = staffRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var staff = await _staffRepository.GetStaffAsync();
            return Ok(staff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] string Id)
    {
        try
        {
            var staff = await _staffRepository.GetStaffAsync(Id);
            return Ok(staff);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }

    public class AddStaffSkillsModel
    {
        [Required(ErrorMessage = "StaffId is required.")]
        public string StaffId { get; set; }

        [Required(ErrorMessage = "At least one SkillId is required.")]
        [MinLength(1, ErrorMessage = "SkillIdList cannot be empty.")]
        public List<Guid> SkillIdList { get; set; }
    }

    [HttpPost("skills")]
    public async Task<IActionResult> AddStaffSkills([FromBody]AddStaffSkillsModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var staff = await _context.Staff
                .Where(x => x.Id == model.StaffId)
                .Include(x => x.Staff_Skills)
                .FirstOrDefaultAsync();

            if (staff is null) return BadRequest($"User: {model.StaffId} not found.");

            foreach (var skillId in model.SkillIdList)
            {
                staff.Staff_Skills.Add(new Staff_Skill
                {
                    SkillId = skillId,
                });
            }

            //await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
