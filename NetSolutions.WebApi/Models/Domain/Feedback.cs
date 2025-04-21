using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Models.Domain;

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
