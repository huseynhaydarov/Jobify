namespace Jobify.Application.UseCases.Users.Commands.CreateEmployers;

public record CreateEmployerCommand(string Email, string Password) : IRequest<UserDto>;
