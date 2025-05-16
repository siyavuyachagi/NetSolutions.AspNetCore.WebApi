namespace Application.DTOs.Response
{
    public class ProjectMilestoneDto
    {
        public Guid Id { get; set; }
        public virtual ProjectDto Project { get; set; }
        public virtual ApplicationUserDto User { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
