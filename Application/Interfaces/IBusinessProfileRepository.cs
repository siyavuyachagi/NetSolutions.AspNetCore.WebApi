using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IBusinessProfileRepository
    {
        Task<BusinessProfileDto> GetBusinessProfileAsync();
    }
}
