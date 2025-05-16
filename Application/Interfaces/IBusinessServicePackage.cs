using Application.DTOs.Response;
using Application.Helpers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBusinessServicePackage
    {
        Task<List<BusinessServicePackageDto>> GetBusinessServicePackagesAsync();
        Task<BusinessServicePackageDto?> GetBusinessServicePackageAsync(Guid id);
        Task<Result> DeleteBusinessServicePackageAsync(Guid id);
        Task CreateBusinessServicePackageAsync(BusinessServicePackage businessServicePackage);
    }
}
