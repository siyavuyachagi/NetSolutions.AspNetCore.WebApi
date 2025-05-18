using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IBusinessProfile
    {
        Task<BusinessProfileDto> GetBusinessProfileAsync();
    }
}
