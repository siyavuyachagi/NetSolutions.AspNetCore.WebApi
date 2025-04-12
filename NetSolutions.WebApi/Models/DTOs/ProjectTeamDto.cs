namespace NetSolutions.WebApi.Models.DTOs;

public class ProjectTeamDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string? Name { get; set; }
    public List<ProjectTeamMemberDto> ProjectTeamMembers { get; set; }
}
