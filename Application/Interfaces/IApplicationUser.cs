using Application.DTOs.Response;
using Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IApplicationUser
    {
        Task<List<ApplicationUserDto>> GetApplicationUsersAsync();
        Task<ApplicationUserDto> GetApplicationUserAsync(string Id);
        Task<ApplicationUserDto> GetApplicationUserByEmailAsync(string Email);
        Task<Result> EditBio(string Id, string? Bio = null);
        Task<Result> EditProfileImage(string Id, IFormFile imageFile);
        Task DeleteApplicationUserAsync(string id);
    }
}
