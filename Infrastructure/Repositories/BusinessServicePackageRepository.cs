using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class BusinessServicePackageRepository : IBusinessServicePackage
    {
        private readonly ILogger<BusinessServicePackageRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BusinessServicePackageRepository(
            ILogger<BusinessServicePackageRepository> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public Task CreateBusinessServicePackageAsync(BusinessServicePackage businessServicePackage)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteBusinessServicePackageAsync(Guid id)
        {
            try
            {
                await _context.BusinessServices
                    .Where(x => x.Id == id)
                    .ExecuteDeleteAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ex.Message); // Add proper error handling
            }
        }

        async Task<BusinessServicePackageDto?> IBusinessServicePackage.GetBusinessServicePackageAsync(Guid id)
        {
            try
            {
                var businessServicePackage = await _context.BusinessServicePackages
                    .AsNoTrackingWithIdentityResolution()
                    .Where(bsp => bsp.Id == id)
                    .Include(bsp => bsp.BusinessService)
                    .Include(bsp => bsp.BusinessServicePackageFeatures)
                    .FirstOrDefaultAsync();
                var result = _mapper.Map<BusinessServicePackageDto>(businessServicePackage);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        async Task<List<BusinessServicePackageDto>> IBusinessServicePackage.GetBusinessServicePackagesAsync()
        {
            try
            {
                var businessServicePackages = await _context.BusinessServicePackages
                    .AsNoTrackingWithIdentityResolution()
                    .Include(bsp => bsp.BusinessService)
                    .Include(bsp => bsp.BusinessServicePackageFeatures)
                    .ToListAsync();
                var result = _mapper.Map<List<BusinessServicePackageDto>>(businessServicePackages);
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
