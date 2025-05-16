using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class SkillsRepository : ISkills
    {
        public Task<SkillDto> CreateSkillAsync(Skill skill)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSkillAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SkillDto> GetSkillAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SkillDto>> GetSkillsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
