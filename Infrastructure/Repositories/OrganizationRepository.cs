using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganization
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrganizationRepository> _logger;
        private readonly IMapper _mapper;

        public OrganizationRepository(
            ApplicationDbContext context, 
            ILogger<OrganizationRepository> logger, 
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateOrganizationAsync(Organization organization)
        {
            try
            {
                _context.Organizations.Add(organization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteOrganizationAsync(Guid id)
        {
            try
            {
                await _context.BusinessServices
                    .Where(bs => bs.Id == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<OrganizationDto> GetOrganizationAsync(Guid id)
        {
            try
            {
                var organization = await _context.Organizations
                    .AsNoTrackingWithIdentityResolution()
                    .Where(o => o.Id == id)
                    .Include(o => o.PhysicalAddress)
                    .Include(o => o.SocialLinks)
                    .FirstOrDefaultAsync();

                var result = _mapper.Map<OrganizationDto>(organization);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<OrganizationDto> GetOrganizationByNameAsync(string name)
        {
            try
            {
                var normalizedName = name.Trim(); // Remove leading/trailing spaces

                var organization = await _context.Organizations
                    .AsNoTrackingWithIdentityResolution()
                    .Where(o => o.Name == normalizedName)
                    .Include(o => o.PhysicalAddress)
                    .Include(o => o.SocialLinks)
                    .FirstOrDefaultAsync();

                var result = _mapper.Map<OrganizationDto>(organization);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<List<OrganizationDto>> GetOrganizationsAsync()
        {
            try
            {
                var organization = await _context.Organizations
                    .AsNoTrackingWithIdentityResolution()
                    .Include(o => o.PhysicalAddress)
                    .Include(o => o.SocialLinks)
                    .ToListAsync();

                var result = _mapper.Map<List<OrganizationDto>>(organization);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
