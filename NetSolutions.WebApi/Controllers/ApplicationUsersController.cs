using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using dotenv.net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using System.ComponentModel.DataAnnotations;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.Validations;
using System.Net;
using NetSolutions.WebApi.Repositories;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationUsersController : ControllerBase
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
    private readonly IApplicationUserRepository _applicationUserRepository;

    public ApplicationUsersController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IJasonWebToken jasonWebToken,
        SmtpSettings smtpSettings,
        JwtSettings jwtSettings,
        Cloudinary cloudinary
,
        IApplicationUserRepository applicationUserRepository)
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
        this._cloudinary = cloudinary;
        _applicationUserRepository = applicationUserRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _applicationUserRepository.GetApplicationUsersAsync();
            if (!result.Succeeded) return NotFound(result.Errors);
            return Ok(result.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] string Id)
    {
        try
        {
            var result = await _applicationUserRepository.GetApplicationUserAsync(Id);
            if (!result.Succeeded) return NotFound(result.Errors);
            return Ok(result.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }


    public class ProfileImageModel
    {
        //[Required, FileExtensions(Extensions = "jpg,jpeg,png"), FileSizeMax(5)]
        public IFormFile ImageFile { get; set; }

    }
    [HttpPatch("profile/image/{UserId}")]
    public async Task<IActionResult> UpdateProfileImage([FromRoute] string UserId, ProfileImageModel model)
    {
        try
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user is null) return NotFound($"User : {UserId}, could not be found!");

            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (model.ImageFile == null || model.ImageFile.Length == 0)
                return BadRequest("No file uploaded.");

            var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Invalid file type.");

            // Upload to Cloudinary
            using var stream = model.ImageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(model.ImageFile.FileName, stream),
                Folder = "profile_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return StatusCode((int)uploadResult.StatusCode, uploadResult.Error?.Message);

            // Delete previous profile image
            var profileImage = await _context.FileMetadatas
                .Where(f => f.ViewLink == user.Avatar)
                .FirstOrDefaultAsync();

            if (profileImage is not null)
            {
                switch (profileImage.StorageProvider)
                {
                    case FileMetadata.EStorageProvider.Local:
                        break;
                    case FileMetadata.EStorageProvider.GoogleDrive:
                        break;
                    case FileMetadata.EStorageProvider.OneDrive:
                        break;
                    case FileMetadata.EStorageProvider.AWS_S3:
                        break;
                    case FileMetadata.EStorageProvider.Cloudinary:
                        await _cloudinary.DeleteResourcesAsync(profileImage.Id);
                        break;
                    default:
                        break;
                }
            }


            // Add new profile
            var fileMetadata = new FileMetadata
            {
                Id = uploadResult.PublicId, // Unique ID assigned by Cloudinary
                Name = Path.GetFileNameWithoutExtension(model.ImageFile.FileName),
                ContentType = model.ImageFile.ContentType,
                Extension = Path.GetExtension(model.ImageFile.FileName),
                Size = model.ImageFile.Length,
                ViewLink = uploadResult.SecureUrl?.ToString(), // Direct view URL
                DownloadLink = uploadResult.SecureUrl?.ToString(), // Same as view link
                StorageProvider = FileMetadata.EStorageProvider.Cloudinary,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.FileMetadatas.Add(fileMetadata);

            // Update link
            user.Avatar = fileMetadata.ViewLink;

            await _context.SaveChangesAsync();
            return Ok(uploadResult.SecureUrl?.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    public class UpdateBioModel
    {
        public string? Bio { get; set; }
    }
    [HttpPost("profile/bio/{UserId}")]
    public async Task<IActionResult> Edit([FromRoute] string UserId, UpdateBioModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _context.Users
                .Where(u => u.Id == UserId)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(u => u.Bio, model.Bio)
                    .SetProperty(u => u.UpdatedAt, DateTime.UtcNow)); // optional: update timestamp
            return Ok();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

}
