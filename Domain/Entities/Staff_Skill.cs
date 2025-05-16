using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Staff_Skill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string StaffId { get; set; }
        [ForeignKey(nameof(StaffId))]
        public virtual Staff Staff { get; set; }

        public Guid SkillId { get; set; }
        [ForeignKey(nameof(SkillId))]
        public virtual Skill Skill { get; set; }

        // Years of experience with this skill
        [Range(0, 50)]
        public int YearsOfExperience { get; set; } = 0;

        // Date when skill was added to profile
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Date when skill was last updated
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }

}
