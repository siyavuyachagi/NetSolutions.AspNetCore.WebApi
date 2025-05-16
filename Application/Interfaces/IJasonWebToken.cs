using Application.DTOs.Response;
using Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IJasonWebToken
    {
        Task<Result<string>> GenerateRefreshToken(string userId);
        Task<Result<AuthDto>> GenerateTokens(string userId, HttpRequest request);
        Task<Result<AuthDto>> RefreshToken(string refreshToken, HttpRequest request);
        int RefreshTokenExpirationMinutes { get; }
    }
}



