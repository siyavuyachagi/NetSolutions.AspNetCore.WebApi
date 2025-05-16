using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<ApplicationUserRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepository(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<ApplicationUserRepository> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task DeleteApplicationUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return;

                // Lock the user indefinitely to simulate soft delete
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

                // Optionally, mark a custom flag for auditing or visibility
                 user.IsDeleted = true;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}: {ErrorMessage}", id, ex.Message);
                throw;
            }
        }


        public async Task<Result> EditBio(string Id, string? Bio)
        {
            try
            {
                var user = await _context.Users
                    .Where(x => x.Id == Id)
                    .FirstOrDefaultAsync();

                if(user is null)
                {
                    _logger.LogInformation("User not found.");
                    return Result.Success("User not found.");
                }

                user.Bio = Bio;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Result> EditProfileImage(string Id, IFormFile imageFile)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user is null) return Result.Failure($"User : {Id}, could not be found!");

            if (imageFile == null || imageFile.Length == 0)
                return Result.Failure("No file uploaded.");

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
                return Result.Failure("Invalid file type.");

            // Upload to Cloudinary
            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "profile_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return Result.Failure(uploadResult.Error.Message);

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
                Name = Path.GetFileNameWithoutExtension(imageFile.FileName),
                ContentType = imageFile.ContentType,
                Extension = Path.GetExtension(imageFile.FileName),
                Size = imageFile.Length,
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

            return Result.Success();
        }

        public async Task<ApplicationUserDto> GetApplicationUserAsync(string Id)
        {
            var user = await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<ApplicationUserDto>(user);
            return result;
        }

        public async Task<ApplicationUserDto> GetApplicationUserByEmailAsync(string Email)
        {
            var user = await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Email == Email)
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .FirstOrDefaultAsync();

            var result = _mapper.Map<ApplicationUserDto>(user);
            return result;
        }

        public async Task<List<ApplicationUserDto>> GetApplicationUsersAsync()
        {
            var users = await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .ToListAsync();

            var results = _mapper.Map<List<ApplicationUserDto>>(users);
            return results;
        }
    }
}
