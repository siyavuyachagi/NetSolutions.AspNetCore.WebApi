using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StaffRepository : IStaff
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StaffRepository(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task AddSkills(List<Staff_Skill> staff_Skills)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StaffDto>> GetStaffAsync()
        {
            var staff = await _context.Staff
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            var result = _mapper.Map<List<StaffDto>>(staff);
            return result;
        }

        public async Task<StaffDto> GetStaffAsync(string Id)
        {
            var staff = await _context.Staff
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Id == Id)
                .Include(x => x.Staff_Skills)
                .ThenInclude(x => x.Skill)
                .Include(x => x.ProjectTeamMembers)
                .ThenInclude(x => x.ProjectTeam)
                .FirstOrDefaultAsync();

            var results = _mapper.Map<StaffDto>(staff);
            return results;
        }
    }
}
