namespace Infrastructure.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string[] Issuers { get; set; }
        public string[] Audiences { get; set; }
        public int TokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
