using Application.DTOs.Response;
using Application.Helpers;

namespace Application.Interfaces
{
    public interface IBusinessService
    {
        Task<List<BusinessServiceDto>> GetBusinessServicesAsync();
        Task<BusinessServiceDto?> GetBusinessServiceAsync(Guid Id);
        Task<Result> DeleteBusinessServiceAsync(Guid Id);
    }
}
