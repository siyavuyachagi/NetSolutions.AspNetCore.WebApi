using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class TechnologyStackRepository : ITechnologyStack
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TechnologyStackRepository> _logger;
        private readonly IMapper _mapper;

        public TechnologyStackRepository(
            ApplicationDbContext context,
            ILogger<TechnologyStackRepository> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TechnologyStackDto> CreateTechnologyStackAsync(TechnologyStack technologyStack)
        {
            try
            {
                _context.TechnologyStacks.Add(technologyStack);
                await _context.SaveChangesAsync();
                return await GetTechnologyStackAsync(technologyStack.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteTechnologyStackAsync(Guid id)
        {
            try
            {
                await _context.BusinessServices.Where(x => x.Id == id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TechnologyStackDto> GetTechnologyStackAsync(Guid id)
        {
            try
            {
                var technologyStack = await _context.TechnologyStacks
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                var result = _mapper.Map<TechnologyStackDto>(technologyStack);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<TechnologyStackDto>> GetTechnologyStacksAsync()
        {
            try
            {
                var technologyStacks = await _context.TechnologyStacks
                    .AsNoTrackingWithIdentityResolution()
                    .ToListAsync();

                var result = _mapper.Map<List<TechnologyStackDto>>(technologyStacks);
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
