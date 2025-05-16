namespace Application.DTOs.Response
{
    public class AuthDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public ApplicationUserDto ApplicationUser { get; set; }
    }
}
