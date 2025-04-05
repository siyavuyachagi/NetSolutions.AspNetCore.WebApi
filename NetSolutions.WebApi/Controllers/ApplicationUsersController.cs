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
    )
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
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.LastName,
                    u.FirstName,
                    u.Email,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.Gender,
                    u.Bio,
                    u.CreatedAt,
                    u.UpdatedAt,
                    Avatar = u.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id) // Match roles by user ID
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                        .ToList(), // Get role names as a list
                    u.Organization,
                    u.UserActivities,
                    Solutions = u.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .ToListAsync();
            return Ok(users);
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
            var users = await _userManager.Users
                .Where(u => u.Id == Id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.LastName,
                    u.FirstName,
                    u.Email,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.Gender,
                    u.Bio,
                    u.CreatedAt,
                    u.UpdatedAt,
                    Avatar = u.ProfileImage.ViewLink,
                    Roles = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id) // Match roles by user ID
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name) // Join with AspNetRoles
                        .ToList(), // Get role names as a list
                    u.Organization,
                    u.UserActivities,
                    Solutions = u.UserSolutions.Select(us => us.Solution).ToList(),
                })
                .FirstOrDefaultAsync();
            return Ok(users);
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
            var user = await _userManager.Users.Include(u => u.ProfileImage).FirstOrDefaultAsync(u => u.Id == UserId);
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
            if(user.ProfileImage is not null)
            {
                switch (user.ProfileImage.StorageProvider)
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
                        // Delete the previous one here
                        var result = await _cloudinary.DeleteResourcesAsync(user.ProfileImage.Id);
                        if (result.StatusCode != HttpStatusCode.OK) 
                            _logger.LogError($"Error deleting a resource from Cloudinary, ResxId: {user.ProfileImage.Id}");
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

            user.ProfileImage = fileMetadata;

            await _context.SaveChangesAsync();
            return Ok(uploadResult.SecureUrl?.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


}
