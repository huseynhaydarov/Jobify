namespace Jobify.Application.UseCases.Auths.Login.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
