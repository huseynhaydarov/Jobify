namespace Jobify.Application.UseCases.Auths.AuthDtos;

public class RefreshTokenResult
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }
}
