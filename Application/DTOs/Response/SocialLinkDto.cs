namespace Application.DTOs.Response
{
    public class SocialLinkDto
    {
        public Guid Id { get; set; }
        public virtual ApplicationUserDto User { get; set; }
        public virtual OrganizationDto Organization { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Handler { get; set; }
        public string IconHTML { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
