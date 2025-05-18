using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class BusinessServiceRepository : IBusinessService
    {
        private readonly ILogger<BusinessServiceRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BusinessServiceRepository(
            ILogger<BusinessServiceRepository> logger, 
            ApplicationDbContext context, 
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> DeleteBusinessServiceAsync(Guid id)
        {
            try
            {
                await _context.BusinessServices.Where(x => x.Id == id)
                    .ExecuteDeleteAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ex.Message); // Add proper error handling
            }
        }

        public async Task<BusinessServiceDto> GetBusinessServiceAsync(Guid Id)
        {
            try
            {
                var businessService = await _context.BusinessServices
                    .AsNoTrackingWithIdentityResolution()
                    .Where(x => x.Id == Id)
                    .Include(x => x.BusinessService_FileMetadata_Images)
                    .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.BusinessService_Testimonials)
                    .ThenInclude(x => x.Testimonial)
                    .ThenInclude(x => x.Evaluator)
                    .Include(x => x.BusinessServicePackages)
                    .ThenInclude(x => x.BusinessServicePackageFeatures)
                    .FirstOrDefaultAsync();

                var response = _mapper.Map<BusinessServiceDto>(businessService);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BusinessServiceDto();
            }
        }

        public async Task<List<BusinessServiceDto>> GetBusinessServicesAsync()
        {
            try
            {
                var businessServices = await _context.BusinessServices
                    .AsNoTrackingWithIdentityResolution()
                    .Include(x => x.BusinessService_FileMetadata_Images)
                    .ThenInclude(x => x.FileMetadata)
                    .Include(x => x.BusinessService_Testimonials)
                    .ThenInclude(x => x.Testimonial)
                    .Include(x => x.BusinessServicePackages)
                    .ToListAsync();

                var response = _mapper.Map<List<BusinessServiceDto>>(businessServices);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new List<BusinessServiceDto>();
            }
        }
    }
}
