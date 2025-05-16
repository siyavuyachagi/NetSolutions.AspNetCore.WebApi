using Application.Helpers;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailsController> _logger;
        private readonly IBusinessProfileRepository _businessProfileRepository;

        public EmailsController(
            IEmailSender emailSender,
            ILogger<EmailsController> logger,
            IBusinessProfileRepository businessProfileRepository)
        {
            _emailSender = emailSender;
            _logger = logger;
            _businessProfileRepository = businessProfileRepository;
        }


        public class ContactUsModel
        {
            public string? LastName { get; set; }
            public string? FirstName { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required, MaxLength(225)]
            public string Subject { get; set; }
            [Required, MaxLength(525)]
            public string Message { get; set; }
        }

        [HttpPost("contact-us")]
        public async Task<IActionResult> ContactUs([FromBody] ContactUsModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var businessProfile = await _businessProfileRepository.GetBusinessProfileAsync();

                var result = await _emailSender.SendEmailAsync(model.Email, businessProfile.Email, $"Email from {model.Email}", $"<p>{model.Message}</p>");
                if (!result.Succeeded)
                {
                    _logger.LogError("Error sending email");
                }

                return Ok(Result.Success("Message sent successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
