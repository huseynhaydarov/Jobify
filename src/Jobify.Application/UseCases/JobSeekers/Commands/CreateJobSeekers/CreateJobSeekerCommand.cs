namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public record CreateJobSeekerCommand(string Email, string Password) : IRequest<JobSeekerDto>;
