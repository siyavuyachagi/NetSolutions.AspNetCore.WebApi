using NetSolutions.WebApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Staff : ApplicationUser
{
    public string StaffNumber { get; set; }

    public Guid ProfessionId { get; set; }
    [ForeignKey(nameof(ProfessionId))]
    public virtual Profession Profession { get; set; }

    public virtual List<Staff_Skill> Skills { get; set; }
    public virtual List<TeamMember> PrjectTeams { get; set; }
}
