using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NetSolutions.WebApi.Models.DTOs
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
