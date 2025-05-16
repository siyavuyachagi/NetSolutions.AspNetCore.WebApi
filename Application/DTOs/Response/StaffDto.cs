namespace Application.DTOs.Response
{
    public class StaffDto : ApplicationUserDto
    {
        public string StaffNumber { get; set; }
        public ProfessionDto Profession { get; set; }
        public List<Staff_SkillDto> Staff_Skills { get; set; }
        public List<ProjectTeamMemberDto> ProjectTeamMembers { get; set; }
    }
}
