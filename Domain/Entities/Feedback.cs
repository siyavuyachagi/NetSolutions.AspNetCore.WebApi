using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string EvaluatorId { get; set; }
        [ForeignKey(nameof(EvaluatorId))]
        public virtual ApplicationUser Evaluator { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp of the review
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}

