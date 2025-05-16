
namespace Domain.Entities
{
    public class HardSkill : Skill
    {
        public virtual List<HardSkill_TechnologyStack> HardSkill_TechnologyStacks { get; set; }
    }
}
