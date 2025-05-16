namespace Application.DTOs.Response
{
    public class ProjectTeamMemberDto
    {
        public Guid Id { get; set; }
        public ProjectTeamDto ProjectTeam { get; set; }
        public StaffDto Member { get; set; }
        public List<TeamMemberRoleDto> TeamMemberRoles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

