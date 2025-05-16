using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class HardSkill_TechnologyStack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid HardSkillId { get; set; }
        [ForeignKey(nameof(HardSkillId))]
        public virtual HardSkill HardSkill { get; set; }

        public Guid TechnologyStackId { get; set; }
        [ForeignKey(nameof(TechnologyStackId))]
        public virtual TechnologyStack TechnologyStack { get; set; }
    }

}

