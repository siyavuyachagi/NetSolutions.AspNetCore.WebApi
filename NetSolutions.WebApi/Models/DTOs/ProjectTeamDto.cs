using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs;

public class ProjectTeamDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<ProjectTeamMemberDto> ProjectTeamMembers { get; set; } = new();
    public virtual ProjectDto Project { get; set; }
    public virtual List<SolutionDto> Solutions { get; set; } = new();
}
