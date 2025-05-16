namespace Application.DTOs.Response
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public virtual ProjectDto Project { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DueAt { get; set; }
        public string Status { get; set; }
    }
}
