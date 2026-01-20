using System.Text;
using Jobify.Application.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace Jobify.IntegrationTests;

public class JwtTokenTestSettings
{
    public const string SecretKey = "z4Tg8/k4ezgNchUg+/dTEQheVNssVS8/c0x4RH0v18w=";

    public const string Audience = "localhost:7045";

    public const string Issuer = "localhost:7045";

    public const int ExpirationMinutes = 15;

    public const int RefreshTokenExpirationDays = 14;

    private static readonly JwtSettings settings = new()
    {
        SecretKey = SecretKey,
        Audience = Audience,
        Issuer = Issuer,
        ExpirationMinutes = ExpirationMinutes,
        RefreshTokenExpirationDays = RefreshTokenExpirationDays
    };

    private static readonly SymmetricSecurityKey s_secretKey = new(
        Encoding.UTF8.GetBytes(settings.SecretKey!));

    public static readonly SigningCredentials Credentials = new(
        s_secretKey,
        SecurityAlgorithms.HmacSha256);
}
