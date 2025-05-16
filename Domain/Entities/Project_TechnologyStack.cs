using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Project_TechnologyStack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public Guid TechnologyStackId { get; set; }
        [ForeignKey(nameof(TechnologyStackId))]
        public TechnologyStack TechnologyStack { get; set; }
    }

}

