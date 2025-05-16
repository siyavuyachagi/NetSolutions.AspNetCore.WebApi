using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProjectTask
    {
        public enum EPriority
        {
            Low,
            Medium,
            High
        }
        public enum EStatus
        {
            Pending,
            Started,
            Complete
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public EPriority Priority { get; set; } = EPriority.Medium;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueAt { get; set; }
        public EStatus Status { get; set; } = EStatus.Pending;
    }

}

