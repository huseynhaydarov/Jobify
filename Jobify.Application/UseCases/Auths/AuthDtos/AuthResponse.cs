namespace Jobify.Application.UseCases.Auths.AuthDtos;

public record AuthResponse(
    bool Success,
    string Token,
    string RefreshToken,
    string? Email = null,
    string? Role = null,
    IEnumerable<string>? Errors = null);


