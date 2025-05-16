using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
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

        public virtual List<ProjectTeamMember_TeamMemberRole> ProjectTeamMember_TeamMemberRoles { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}

