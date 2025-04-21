using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class ProjectTeamMember
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProjectTeamId { get; set; }
    [ForeignKey(nameof(ProjectTeamId))]
    public virtual ProjectTeam ProjectTeam { get; set; }

    public string MemberId { get; set; }
    [ForeignKey(nameof(MemberId))]
    public virtual Staff Member { get; set; }

    public virtual List<ProjectTeamMember_TeamMemberRole> TeamMemberRoles { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
