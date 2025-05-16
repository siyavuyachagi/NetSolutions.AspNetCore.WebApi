using Application.DTOs.Response;
using Application.Helpers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectTeamMember
    {
        Task<List<ProjectTeamMemberDto>> GetProjectTeamMembersAsync();
        Task<ProjectTeamMemberDto> GetProjectTeamMemberAsync(Guid Id);
        Task<ProjectTeamMemberDto> CreateProjectTeamMemberAsync(ProjectTeamMember projectTeam);
        Task DeleteProjectTeamAsync(Guid Id);
    }
}
