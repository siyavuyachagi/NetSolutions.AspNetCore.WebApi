using NetSolutions.Helpers;
using NetSolutions.WebApi.Models.DTOs;

namespace NetSolutions.WebApi.Repositories;

public interface IOrganizationRepository
{
    Task<Result<List<OrganizationDto>>> GetOrganizationsAsync();
}
