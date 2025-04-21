using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Controllers;
using NetSolutions.WebApi.Models.DTOs;
using AutoMapper;
using NetSolutions.WebApi.Repositories;

namespace NetSolutions.Services;

#region Interface
public interface IJasonWebToken
{
    Task<Result<string>> GenerateRefreshToken(string userId);
    Task<Result<AuthResponseDto>> GenerateTokens(string userId, HttpRequest request);
    Task<Result<AuthResponseDto>> RefreshToken(string refreshToken, HttpRequest request);
    int RefreshTokenExpirationMinutes { get; }
}
#endregion


#region Implementation
public class JasonWebToken : IJasonWebToken
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IJasonWebToken> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;
    private readonly IApplicationUserRepository _applicationUserRepository;


    public JasonWebToken(
        ApplicationDbContext dbContext,
        ILogger<IJasonWebToken> logger,
        UserManager<ApplicationUser> userManager,
        JwtSettings jwtSettings,
        IApplicationUserRepository applicationUserRepository)
    {
        _context = dbContext;
        _logger = logger;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        // Initialize the refresh token expiration days from the settings
        RefreshTokenExpirationMinutes = jwtSettings.RefreshTokenExpirationMinutes;
        _applicationUserRepository = applicationUserRepository;
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
                _context.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                return Result.Success(token);
            }
        }
        catch (Exception ex)
        {
            return Result.Failed<string>(ex.Message);
        }
    }

    #endregion

    public async Task<Result<AuthResponseDto>> GenerateTokens(string userId, HttpRequest request)
    {
        try
        {
            var user = await _applicationUserRepository.GetApplicationUserAsync(userId);

            if (user is null) return Result.Failed<AuthResponseDto>($"User: {userId} not found.");

            // Create the initial claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Add role claims
            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create the key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // Create the credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: $"{request.Scheme}://{request.Host}{request.PathBase}",
                audience: request.Headers.Origin,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationMinutes),
                signingCredentials: creds);

            // Return the serialized token
            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshTokenResult = await GenerateRefreshToken(userId);
            if (!refreshTokenResult.Succeeded) throw new Exception("Error generating refresh token");


            var tokensResponse = new AuthResponseDto
            {
                AccessToken = serializedToken,
                RefreshToken = refreshTokenResult.Response,
                ApplicationUser = _mapper.Map<ApplicationUserDto>(user)
            };
            return Result.Success(tokensResponse);
        }
        catch (Exception ex)
        {
            return Result.Failed<AuthResponseDto>(ex.Message);
        }
    }


    public async Task<Result<AuthResponseDto>> RefreshToken(string refreshToken, HttpRequest request)
    {
        try
        {
            // Retrieve the stored refresh token
            var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                return Result.Failed<AuthResponseDto>("Invalid refresh token.");

            // Check if token has expired
            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                return Result.Failed<AuthResponseDto>("Session expired.");

            // Mark the token as used
            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            // Generate a new JWT Access & Refresh tokens
            var result = await GenerateTokens(storedRefreshToken.UserId, request);

            return Result.Success(result.Response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failed<AuthResponseDto>(ex.Message);
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

#endregion



