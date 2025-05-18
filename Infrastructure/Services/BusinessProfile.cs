
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class BusinessProfileRepository : IBusinessProfile
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BusinessProfileRepository> _logger;

        public BusinessProfileRepository(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<BusinessProfileRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BusinessProfileDto> GetBusinessProfileAsync()
        {
            try
            {
                var businessProfile = await _context.BusinessProfile
                    .AsNoTracking()
                    .Include(x => x.PhysicalAddress)
                    .Include(x => x.SocialLinks)
                    .FirstOrDefaultAsync();

                var dto = _mapper.Map<BusinessProfileDto>(businessProfile);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
