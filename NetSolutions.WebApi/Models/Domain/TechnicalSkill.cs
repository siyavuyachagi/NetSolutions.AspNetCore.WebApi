using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class TechnicalSkill : UserSkill
{
    public virtual List<TechnicalSkill_TechnologyStack> TechnicalSkill_TechnologyStacks { get; set; }
    public virtual List<Certification> Certifications { get; set; }
}