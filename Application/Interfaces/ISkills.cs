using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISkills
    {
        Task<List<SkillDto>> GetSkillsAsync();
        Task<SkillDto> GetSkillAsync(Guid id);
        Task<SkillDto> CreateSkillAsync(Skill skill);
        Task DeleteSkillAsync(Guid id);
    }
}
