using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Solution_TechnologyStack
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid SolutionId { get; set; }
    [ForeignKey(nameof(SolutionId))]
    public Solution Solution { get; set; }

    public Guid TechnologyStackId { get; set; }
    [ForeignKey(nameof(TechnologyStackId))]
    public TechnologyStack TechnologyStack { get; set; }
}
