using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Staff_UserSkill
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string StaffId { get; set; }
    [ForeignKey(nameof(StaffId))]
    public virtual Staff Staff { get; set; }

    public Guid UserSkillId { get; set; }
    [ForeignKey(nameof(UserSkillId))]
    public virtual UserSkill UserSkill { get; set; }

    // Years of experience with this skill
    [Range(0, 50)]
    public int YearsOfExperience { get; set; } = 0;

    // Date when skill was added to profile
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Date when skill was last updated
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    // Optional endorsements from colleagues
    public int EndorsementCount { get; set; } = 0;

    // Determines if this skill is featured on the staff profile
    public bool IsFeatured { get; set; } = true;
}