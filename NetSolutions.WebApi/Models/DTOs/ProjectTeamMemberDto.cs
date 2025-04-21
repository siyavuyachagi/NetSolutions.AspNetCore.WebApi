using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class ProjectTeamMemberDto
{
    public Guid Id { get; set; }
    public virtual ProjectTeamDto ProjectTeam { get; set; }
    public virtual StaffDto Staff { get; set; }
    public List<TeamMemberRoleDto> TeamMemberRoles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}