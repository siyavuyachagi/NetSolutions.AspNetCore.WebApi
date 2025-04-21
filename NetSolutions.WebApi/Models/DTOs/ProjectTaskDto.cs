using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }
        public virtual ProjectDto Project { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public string Priority { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueAt { get; set; }
        public string Status { get; set; }
    }
}
