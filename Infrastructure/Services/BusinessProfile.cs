
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class BusinessProfile : IBusinessProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BusinessProfile(
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

                throw;
            }
        }
    }
}
