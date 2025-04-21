namespace NetSolutions.WebApi.Models.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public ApplicationUserDto ApplicationUser { get; set; }
    }
}
