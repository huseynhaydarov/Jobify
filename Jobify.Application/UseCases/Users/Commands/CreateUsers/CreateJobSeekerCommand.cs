namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public record CreateJobSeekerCommand(string Email, string Password) : IRequest<Guid>;
