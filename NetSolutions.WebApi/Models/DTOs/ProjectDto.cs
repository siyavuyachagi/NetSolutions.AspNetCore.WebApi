using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; }
    public BusinessService BusinessService { get; set; }
    public Client Client { get; set; }
    public List<ProjectTask> ProjectTasks { get; set; }
    public List<ProjectMilestone> ProjectMilestones { get; set; }
    public List<Solution> Solutions { get; set; }
    public List<FileMetadata> Documents { get; set; }
    public List<TechnologyStack> TechnologyStacks { get; set; }
    public ProjectTeamDto ProjectTeam { get; set; }
    public string Discriminator { get; set; }
}
