using Application.Interfaces;
using Domain.Entities;
using Domain.Validations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessServicesController : ControllerBase
    {
        private readonly ILogger<BusinessServicesController> _logger;
        private readonly IBusinessService _businessServiceRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGuestUser _guestUser;

        public BusinessServicesController(
            ILogger<BusinessServicesController> logger,
            IBusinessService businessServiceRepository,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IGuestUser guestUser)
        {
            _logger = logger;
            _businessServiceRepository = businessServiceRepository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _guestUser = guestUser;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var businessServices = await _businessServiceRepository.GetBusinessServicesAsync();
                return Ok(businessServices);
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

                var businessService = await _businessServiceRepository.GetBusinessServiceAsync(Id);
                return Ok(businessService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }



        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            try
            {
                await _businessServiceRepository.DeleteBusinessServiceAsync(Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //[RequireUserAccount]
        public class TestimonialRequest
        {
            public string? LastName { get; set; }
            public string? FirstName { get; set; }
            [Required]
            public string Email { get; set; }

            [Range(1, 5)]
            public int Rating { get; set; }

            [Required]
            public string Comment { get; set; }
        }

        [HttpPost("{id}/testimonials")]
        public async Task<IActionResult> CreateTestimonial([FromRoute] Guid id, [FromBody] TestimonialRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var user = await _userManager.FindByEmailAsync(request.Email);
                string userId;
                if (user is null)
                {
                    var newUser = new GuestUser()
                    {
                        UserName = request.Email,
                        Email = request.Email,
                        LastName = request.LastName,
                        FirstName = request.FirstName,
                    };

                    var result = await _guestUser.CreateUserAsync(newUser);
                    if (!result.Succeeded) return BadRequest(result.Errors);
                    userId = newUser.Id;
                }
                else
                {
                    userId = user.Id;
                }

                    var businessService = await _context.BusinessServices
                        .Where(bs => bs.Id == id)
                        .Include(bs => bs.BusinessService_Testimonials)
                        .FirstOrDefaultAsync();

                if (businessService is null) return NotFound($"Service :{id} not found.");

                var businessService_Testimonial = new BusinessService_Testimonial
                {
                    Testimonial = new Testimonial
                    {
                        Rating = request.Rating,
                        Comment = request.Comment,
                        EvaluatorId = userId,
                    }
                };

                businessService.BusinessService_Testimonials.Add(businessService_Testimonial);

                await _context.SaveChangesAsync();
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
