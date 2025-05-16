namespace Application.DTOs.Response
{
    public class ProjectTeamDto : TeamDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public virtual ProjectDto Project { get; set; }
        public virtual List<ProjectTeamMemberDto> ProjectTeamMembers { get; set; }
        public virtual List<SolutionDto> Solutions { get; set; }
    }
}

