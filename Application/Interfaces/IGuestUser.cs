using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IGuestUser
    {
        Task<IdentityResult> CreateUserAsync(GuestUser user, string? password = null);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}
