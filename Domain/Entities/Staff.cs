using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Staff : ApplicationUser
    {
        public string StaffNumber { get; set; }

        public Guid ProfessionId { get; set; }
        [ForeignKey(nameof(ProfessionId))]
        public virtual Profession Profession { get; set; }

        public virtual List<Staff_Skill> Staff_Skills { get; set; }
        public virtual List<ProjectTeamMember> ProjectTeamMembers { get; set; }
    }
}

