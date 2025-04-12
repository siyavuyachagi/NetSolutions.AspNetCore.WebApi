using Microsoft.AspNetCore.Identity;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Repositories;

public interface IUserRolesRepository
{
    Task<Result> GetUserRolesAsync();
    Task<Result> GetUserRoleAsync(string Id);
    Task<Result> CreateUserRolesAsync(string Name);
    Task<Result> DeleteUserRolesAsync();
}

public class UserRolesRepository : IUserRolesRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserRolesRepository(
        RoleManager<IdentityRole> roleManager, 
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result> CreateUserRolesAsync(string name)
    {
        if (await _roleManager.RoleExistsAsync(name))
            return Result.Success("Role already exists.");

        var result = await _roleManager.CreateAsync(new IdentityRole(name));

        if (result.Succeeded)
            return Result.Success();

        // Convert IdentityError to a string array
        var errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToArray();
        return Result.Failed(errors);
    }


    public Task<Result> DeleteUserRolesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> GetUserRoleAsync(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> GetUserRolesAsync()
    {
        throw new NotImplementedException();
    }
}
