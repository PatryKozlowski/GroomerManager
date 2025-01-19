using GroomerManager.Domain.DTOs;

namespace GroomerManager.Application.Common.Interfaces;

public interface IJwtManager
{
    bool ValidateToken(string token, bool isRefreshToken);
    string GenerateUserToken(UserDto user, bool isRefreshToken);
    string? GetClaim(string token, string claimType);
}