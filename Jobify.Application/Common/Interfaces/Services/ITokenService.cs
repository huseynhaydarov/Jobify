using System.Security.Claims;
using Jobify.Domain.Entities;

namespace Jobify.Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
