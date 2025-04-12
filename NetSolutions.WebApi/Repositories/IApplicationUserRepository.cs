using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<Result<List<ApplicationUserDto>>> GetApplicationUsersAsync();
        Task<Result<ApplicationUserDto?>> GetApplicationUserAsync(string Id);
        Task<Result<ApplicationUserDto?>> GetApplicationUserByUserNameAsync(string UserName);
        Task<Result> DeleteApplicationUserAsync(string Id);
        Task<Result> EditApplicationUserAsync(ApplicationUser applicationUser);
    }

    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationUserRepository> _logger;
        private readonly IRedisCache _redisCache;
        private const string APPLICATION_USERS_CACHE_KEY = "application_users_list_cache";

        public ApplicationUserRepository(
            ApplicationDbContext context,
            ILogger<ApplicationUserRepository> logger,
            IRedisCache redisCache)
        {
            _context = context;
            _logger = logger;
            _redisCache = redisCache;
        }

        public async Task<Result> DeleteApplicationUserAsync(string id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.IsDeleted, true));

            await RefreshCacheAsync();

            return Result.Success();
        }

        public async Task<Result> EditApplicationUserAsync(ApplicationUser applicationUser)
        {
            try
            {
                _context.Users.Update(applicationUser);
                await _context.SaveChangesAsync();

                await RefreshCacheAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<ApplicationUserDto?>> GetApplicationUserAsync(string Id)
        {
            try
            {
                var cacheApplicationUsers = await _redisCache.GetAsync<List<ApplicationUserDto>>(APPLICATION_USERS_CACHE_KEY) ?? [];
                if (cacheApplicationUsers.Count != 0)
                {
                    return Result.Success(cacheApplicationUsers.Where(u => u.Id == Id).FirstOrDefault());
                }
                else
                {
                    var newApplicationUsers = await RefreshCacheAsync();
                    return Result.Success(newApplicationUsers.Where(u => u.Id == Id).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (Result<ApplicationUserDto?>)Result.Failed(ex.Message);
            }
        }


        public async Task<Result<List<ApplicationUserDto>>> GetApplicationUsersAsync()
        {
            try
            {
                var cacheApplicationUsers = await _redisCache.GetAsync<List<ApplicationUserDto>>(APPLICATION_USERS_CACHE_KEY) ?? [];
                if (cacheApplicationUsers.Count != 0)
                {
                    return Result.Success(cacheApplicationUsers);
                }
                else
                {
                    var newApplicationUsers = await RefreshCacheAsync();
                    return Result.Success(newApplicationUsers);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<List<ApplicationUserDto>> RefreshCacheAsync()
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .AsNoTrackingWithIdentityResolution()
                    .Join(_context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => new { ur.UserId, RoleName = r.Name })
                    .GroupBy(x => x.UserId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleName).ToList());

                var applicationUsers = await _context.Users
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => !x.IsDeleted)
                    .Include(u => u.Organization)
                    .Select(u => new ApplicationUserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        LastName = u.LastName,
                        FirstName = u.FirstName,
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        PhoneNumber = u.PhoneNumber,
                        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                        Gender = u.Gender,
                        Bio = u.Bio,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt,
                        Avatar = u.Avatar,
                        Roles = userRoles.ContainsKey(u.Id) ? userRoles[u.Id] : new List<string>(),
                        Organization = u.Organization,
                        UserActivities = u.UserActivities,
                        Solutions = u.UserSolutions.Select(us => us.Solution).ToList()
                    })
                    .ToListAsync();

                //Save cache
                if (applicationUsers.Count != 0) await _redisCache.SetAsync(APPLICATION_USERS_CACHE_KEY, applicationUsers);

                return applicationUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<Result<ApplicationUserDto?>> GetApplicationUserByUserNameAsync(string UserName)
        {
            try
            {
                var cacheApplicationUsers = await _redisCache.GetAsync<List<ApplicationUserDto>>(APPLICATION_USERS_CACHE_KEY) ?? [];
                if (cacheApplicationUsers.Count != 0)
                {
                    return Result.Success(cacheApplicationUsers.Where(u => u.UserName == UserName).FirstOrDefault());
                }
                else
                {
                    var newApplicationUsers = await RefreshCacheAsync();
                    return Result.Success(newApplicationUsers.Where(u => u.UserName == UserName).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (Result<ApplicationUserDto?>)Result.Failed(ex.Message);
            }
        }
    }
}
