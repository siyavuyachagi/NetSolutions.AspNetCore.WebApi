namespace Application.DTOs.Response
{
    public class Staff_SkillDto
    {
        public Guid Id { get; set; }
        public virtual StaffDto Staff { get; set; }
        public virtual StaffDto Skill { get; set; }
        public int YearsOfExperience { get; set; }
        // Date when skill was added to profile
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Date when skill was last updated
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
