using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class ProjectTeamMember_TeamMemberRole
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid ProjectTeamMemberId { get; set; }
    [ForeignKey(nameof(ProjectTeamMemberId))]
    public ProjectTeamMember TeamMember { get; set; }

    public Guid TeamMemberRoleId { get; set; }
    [ForeignKey(nameof(TeamMemberRoleId))]
    public TeamMemberRole TeamMemberRole { get; set; }
}
