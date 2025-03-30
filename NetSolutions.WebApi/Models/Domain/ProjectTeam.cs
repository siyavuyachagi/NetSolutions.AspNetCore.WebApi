using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class ProjectTeam
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public virtual Project Project { get; set; }

    public string? Name { get; set; }
    public virtual List<TeamMember> TeamMembers { get; set; }
    public virtual List<Solution> Solutions { get; set; }
}
