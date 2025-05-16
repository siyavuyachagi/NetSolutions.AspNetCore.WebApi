using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SolutionFeature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SolutionId { get; set; }
        [ForeignKey(nameof(SolutionId))]
        public virtual Solution Solution { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
    }

}

