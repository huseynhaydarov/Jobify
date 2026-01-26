using System;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.Domain.Enums;
using Jobify.Contracts.JobListings.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting,
    IRequestHandler<DeleteJobListingCommand, CloseJobListingResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteJobListingCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService, IDistributedCache cache,
        ILogger<DeleteJobListingCommandHandler> logger, IPublishEndpoint publishEndpoint) : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CloseJobListingResponse> Handle(DeleteJobListingCommand request,
        CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
                             .FirstOrDefaultAsync(x => x.Id == request.Id
                                                       && x.CreatedBy == _authenticatedUserService.Id,
                                 cancellationToken)
                         ?? throw new NotFoundException("JobListing not found");

        jobListing.IsDeleted = true;
        jobListing.Status = JobStatus.Closed;
        jobListing.ClosedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new JobListingDeletedEvent
        {
            Id = jobListing.Id,
            ClosedAt = jobListing.ModifiedAt ??  DateTimeOffset.UtcNow,
            ClosedById = jobListing.ModifiedBy ?? Guid.Empty,
            ClosedBy = _authenticatedUserService.Email
        }, cancellationToken);

        return new CloseJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ClosedAt);
    }
}
