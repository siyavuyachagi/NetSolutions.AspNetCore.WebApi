using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ProjectTeam : Team
    {
        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }
        public virtual List<ProjectTeamMember> ProjectTeamMembers { get; set; }
        public virtual List<Solution> Solutions { get; set; }
    }

}

