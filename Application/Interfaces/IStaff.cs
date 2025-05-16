
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStaff
    {
        Task<List<StaffDto>> GetStaffAsync();
        Task<StaffDto> GetStaffAsync(string Id);
        Task AddSkills(List<Staff_Skill> staff_Skills);
    }
}
