using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class ProjectTeamMemberDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TeamMemberRole> TeamMemberRoles { get; set; }
}
