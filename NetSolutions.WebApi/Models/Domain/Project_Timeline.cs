using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Project_Timeline
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    public Guid TimelineId { get; set; }
    [ForeignKey(nameof(TimelineId))]
    public virtual Timeline Timeline { get; set; }
}
