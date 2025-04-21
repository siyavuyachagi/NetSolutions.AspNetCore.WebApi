using NetSolutions.WebApi.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetSolutions.WebApi.Models.DTOs
{
    public class SolutionFeatureDto
    {
        public Guid Id { get; set; }
        public virtual Solution Solution { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
