using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class TechnicalSkill_TechnologyStack
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid TechnicalSkillId { get; set; }
    [ForeignKey(nameof(TechnicalSkillId))]
    public virtual TechnicalSkill TechnicalSkill { get; set; }

    public Guid TechnologyStackId { get; set; }
    [ForeignKey(nameof(TechnologyStackId))]
    public virtual TechnologyStack TechnologyStack { get; set; }
}
