using MediatR;

namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public record CreateUserCommand(string Username, string Email, string Password) : IRequest<Guid>;
