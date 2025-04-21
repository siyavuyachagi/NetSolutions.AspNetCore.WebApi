using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.WebApi.Models.DTOs;

public class StaffDto: ApplicationUserDto
{
    public string StaffNumber { get; set; }
    public ProfessionDto Profession { get; set; }
    public List<UserSkillDto> UserSkills { get; set; }
}
