using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.JobApplications.Dtos;
using Jobify.Domain.Entities;
using Jobify.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandHandler : BaseSetting,
    IRequestHandler<CreateJobApplicationCommand, JobApplicationDto>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger<CreateJobApplicationCommandHandler> _logger;

    public CreateJobApplicationCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CreateJobApplicationCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _logger = logger;
    }

    public async Task<JobApplicationDto> Handle(CreateJobApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _authenticatedUserService.Id
                     ?? throw new UnauthorizedException("User is not authenticated");

        var submittedApplication = await _dbContext.JobApplications
            .AnyAsync(a => a.JobListingId == request.JobListingId &&
                           a.ApplicantId == userId, cancellationToken);

        if (submittedApplication)
        {
            throw new DomainException("You have already applied to this job.");
        }

        var jobListing = await _dbContext.JobListings
            .Where(l => l.Id == request.JobListingId && !l.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (jobListing == null)
        {
            throw new NotFoundException("Job listing not found.");
        }

        if (jobListing.Status != JobStatus.Open)
        {
            throw new DomainException("Can apply only to open job.");
        }

        JobApplication application = new()
        {
            JobListingId = request.JobListingId,
            ApplicantId = userId,
            AppliedAt = DateTimeOffset.UtcNow,
            ApplicationStatus = ApplicationStatus.Applied
        };

        await _dbContext.JobApplications.AddAsync(application, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created job application by user {application.ApplicantId}");

        return new JobApplicationDto(application.Id, JobStatus.Open);
    }
}
