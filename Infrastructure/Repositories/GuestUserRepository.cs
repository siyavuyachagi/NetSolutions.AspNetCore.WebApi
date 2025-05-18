using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class GuestUserRepository : IGuestUser
    {
        private readonly ILogger<GuestUserRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GuestUserRepository(
            ILogger<GuestUserRepository> logger, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUserAsync(GuestUser user, string? password)
        {
            try
            {
                var result = !string.IsNullOrEmpty(password)
                    ? await _userManager.CreateAsync(user, password)
                    : await _userManager.CreateAsync(user);

                if (!result.Succeeded) return result;

                if (!await _roleManager.RoleExistsAsync(nameof(GuestUser)))
                    await _roleManager.CreateAsync(new IdentityRole(nameof(GuestUser)));

                await _userManager.AddToRoleAsync(user, nameof(GuestUser));
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserCreationError",
                    Description = $"An error occurred while creating the user: {ex.Message}"
                });
            }
        }


        public Task<IdentityResult> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
