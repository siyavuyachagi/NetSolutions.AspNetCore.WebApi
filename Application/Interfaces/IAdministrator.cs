
using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IAdministrator
    {
        Task<AdministratorDto> GetAdministratorAsync(string Id);
        Task<List<AdministratorDto>> GetAdministratorsAsync();
    }
}
