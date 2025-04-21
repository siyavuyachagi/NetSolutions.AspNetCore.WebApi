using AutoMapper;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<List<ApplicationUserDto>> GetApplicationUsersAsync();
        Task<ApplicationUserDto> GetApplicationUserAsync(string Id);
    }

    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ApplicationUserRepository(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<List<ApplicationUserDto>> GetApplicationUsersAsync()
        {
            var users = await _context.Users
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.IdentityUserRoles)
                .Include(x => x.IdentityRoles)
                .Include(x => x.PhysicalAddress)
                .Include(x => x.UserActivities)
                .FirstOrDefaultAsync();

            var results = _mapper.Map<List<ApplicationUserDto>>(users);
            return results;
        }
    }
}
