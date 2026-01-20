using System;
using Jobify.Application.UseCases.JobApplications.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public record CreateJobApplicationCommand(
    Guid JobListingId,
    string? CoverLetter) : IRequest<JobApplicationDto>;
