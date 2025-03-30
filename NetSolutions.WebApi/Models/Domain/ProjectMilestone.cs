using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Models.Domain;

public class ProjectMilestone
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
