using Application.DTOs.Response;
using Application.Helpers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectTeam
    {
        Task<List<ProjectTeamDto>> GetProjectTeamsAsync();
        Task<ProjectTeamDto> GetProjectTeamAsync(Guid Id);
        Task<ProjectTeamDto> CreateProjectTeamAsync(ProjectTeam projectTeam);
        Task<Result> DeleteProjectTeamAsync(Guid Id);
    }
}
