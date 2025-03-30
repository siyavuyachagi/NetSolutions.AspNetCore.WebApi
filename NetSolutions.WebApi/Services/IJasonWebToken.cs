using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NetSolutions.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;

namespace NetSolutions.Services;

#region Interface
public interface IJasonWebToken
{
    Task<Result<string>> GenerateRefreshToken(string userId);
    Task<Result<TokenResponse>> GenerateTokens(ApplicationUser user, string[] roles);
    Task<Result<TokenResponse>> RefreshToken(string refreshToken);
    int RefreshTokenExpirationMinutes { get; }
}
#endregion


#region Implementation
public class JasonWebToken : IJasonWebToken
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<IJasonWebToken> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    public JasonWebToken(
        ApplicationDbContext dbContext,
        ILogger<IJasonWebToken> logger,
        UserManager<ApplicationUser> userManager,
        JwtSettings jwtSettings
    )
    {
        _db = dbContext;
        _logger = logger;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        // Initialize the refresh token expiration days from the settings
        RefreshTokenExpirationMinutes = jwtSettings.RefreshTokenExpirationMinutes;
    }

    public int RefreshTokenExpirationMinutes { get; }  // Refresh token expiry



    #region Methods
    public async Task<Result<string>> GenerateRefreshToken(string userId)
    {
        try
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);

                // Save the refresh token to the database
                var refreshToken = new RefreshToken
                {
                    Token = token,
                    ExpiryDate = DateTime.UtcNow.AddMinutes(RefreshTokenExpirationMinutes),
                    UserId = userId,
                    IsUsed = false,
                    IsRevoked = false
                };
                _db.RefreshTokens.Add(refreshToken);
                await _db.SaveChangesAsync();

                return Result.Success(token);
            }
        }
        catch (Exception ex)
        {
            return Result.Failed<string>(ex.Message);
        }
    }

    #endregion

    public async Task<Result<TokenResponse>> GenerateTokens(ApplicationUser user, string[] roles)
    {
        try
        {
            // Create the initial claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create the key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // Create the credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuers[0],
                audience: _jwtSettings.Audiences[0],
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationMinutes),
                signingCredentials: creds);

            // Return the serialized token
            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshTokenResult = await GenerateRefreshToken(user.Id);
            var avatar = await _db.Users
                .Where(u => u.Id == user.Id)
                .Include(pi => pi.ProfileImage)
                .Select(pi => pi.ProfileImage.ViewLink)
                .FirstOrDefaultAsync();
            var tokensResponse = new TokenResponse
            {
                AccessToken = serializedToken,
                RefreshToken = refreshTokenResult.Response,
                User = new
                {
                    user.Id,
                    user.UserName,
                    user.LastName,
                    user.FirstName,
                    Roles = roles,
                    avatar
                }
            };
            return Result.Success(tokensResponse);
        }
        catch (Exception ex)
        {
            return Result.Failed<TokenResponse>(ex.Message);
        }
    }


    public async Task<Result<TokenResponse>> RefreshToken(string refreshToken)
    {
        try
        {
            // Retrieve the stored refresh token
            var storedRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                return Result.Failed<TokenResponse>("Invalid refresh token.");

            // Check if token has expired
            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                return Result.Failed<TokenResponse>("Session expired.");

            // Mark the token as used
            storedRefreshToken.IsUsed = true;
            _db.RefreshTokens.Update(storedRefreshToken);
            await _db.SaveChangesAsync();

            // Generate a new JWT Access & Refresh tokens
            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            var tokensResult = await GenerateTokens(user, roles.ToArray());

            return Result.Success(tokensResult.Response);
        }
        catch (Exception ex)
        {
            return Result.Failed<TokenResponse>(ex.Message);
        }
    }
}

#endregion


#region Models


public class JwtSettings
{
    public string SecretKey { get; set; }
    public string[] Issuers { get; set; }
    public string[] Audiences { get; set; }
    public int TokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}


public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public object User { get; set; }
}

#endregion



