namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public record CreateUserCommand(string Username, string Email, string Password, Guid RoleId) : IRequest<Guid>;
