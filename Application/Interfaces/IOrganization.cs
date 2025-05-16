using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrganization
    {
        Task<List<OrganizationDto>> GetOrganizationsAsync();
        Task<OrganizationDto> GetOrganizationAsync(Guid id);
        Task<OrganizationDto> GetOrganizationByNameAsync(string name);
        Task CreateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(Guid id);
    }
}
