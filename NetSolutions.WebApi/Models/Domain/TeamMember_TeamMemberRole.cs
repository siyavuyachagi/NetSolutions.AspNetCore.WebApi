using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.Domain;

public class TeamMember_TeamMemberRole
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid TeamMemberId { get; set; }
    [ForeignKey(nameof(TeamMemberId))]
    public TeamMember TeamMember { get; set; }

    public Guid TeamMemberRoleId { get; set; }
    [ForeignKey(nameof(TeamMemberRoleId))]
    public TeamMemberRole TeamMemberRole { get; set; }
}
