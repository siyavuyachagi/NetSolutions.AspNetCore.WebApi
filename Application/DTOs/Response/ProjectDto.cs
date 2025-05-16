namespace Application.DTOs.Response;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; }
    public BusinessServiceDto? BusinessService { get; set; }
    public ClientDto Client { get; set; }
    public ProjectTeamDto ProjectTeam { get; set; }
    public TimelineDto Timeline { get; set; }
    public List<FileMetadataDto> Documents { get; set; }
    public List<ProjectTaskDto> ProjectTasks { get; set; }
    public List<ProjectMilestoneDto> ProjectMilestones { get; set; }
    public List<TechnologyStackDto> TechnologyStacks { get; set; }
    public List<SolutionDto> Solutions { get; set; }
    public string Discriminator { get; set; }
}
