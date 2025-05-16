using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class AdministratorRepository : IAdministrator
    {
        private readonly ILogger<AdministratorRepository> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AdministratorRepository(
            ILogger<AdministratorRepository> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AdministratorDto> GetAdministratorAsync(string Id)
        {
            try
            {
                var admin = await context.Administrators
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == Id)
                    .Include(x => x.IdentityUserRoles)
                    .Include(x => x.IdentityRoles)
                    .Include(x => x.PhysicalAddress)
                    .Include(x => x.UserActivities)
                    .FirstOrDefaultAsync();

                var dto = mapper.Map<AdministratorDto>(admin);
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<AdministratorDto>> GetAdministratorsAsync()
        {
            try
            {
                var admins = await context.Administrators
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.IdentityUserRoles)
                    .Include(x => x.IdentityRoles)
                    .Include(x => x.PhysicalAddress)
                    .Include(x => x.UserActivities)
                    .ToListAsync();

                var dto = mapper.Map<List<AdministratorDto>>(admins);
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
