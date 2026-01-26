using Jobify.Application.UseCases.JobSeekers.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public record CreateJobSeekerCommand(string Email, string Password) : IRequest<JobSeekerDto>;
