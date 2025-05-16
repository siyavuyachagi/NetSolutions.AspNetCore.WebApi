namespace Application.DTOs.Response
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public virtual ApplicationUserDto ApplicationUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
