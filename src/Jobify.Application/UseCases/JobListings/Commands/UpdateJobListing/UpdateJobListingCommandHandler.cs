using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.Application.UseCases.JobListings.Events;
using Jobify.Contracts.JobListings.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting,
    IRequestHandler<UpdateJobListingCommand, UpdateJobListingResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateJobListingCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateJobListingCommandHandler(
        IApplicationDbContext dbContext,
        ILogger<UpdateJobListingCommandHandler> logger,
        IDistributedCache cache,
        IAuthenticatedUserService authenticatedUserService, IPublishEndpoint publishEndpoint) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
        _authenticatedUserService = authenticatedUserService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<UpdateJobListingResponse> Handle(UpdateJobListingCommand request,
        CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
                             .Where(c => c.Id == request.Id &&
                                         c.CreatedBy == _authenticatedUserService.Id)
                             .FirstOrDefaultAsync(cancellationToken)
                         ?? throw new NotFoundException("JobListing not found");

        jobListing.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var jobListingUpdatedEvent = new JobListingChangedEvent { Id = jobListing.Id, Action = ActionType.Updated };

        await _publishEndpoint.Publish(jobListingUpdatedEvent, cancellationToken);

        await _publishEndpoint.Publish(
            new JobListingUpdated
            {
                Id = jobListing.Id,
                Name = jobListing.Name,
                Description = jobListing.Description,
                Requirements = jobListing.Requirements,
                Location = jobListing.Location,
                Salary = jobListing.Salary,
                Status = jobListing.Status.ToString()
            }, cancellationToken);

        var cacheKey = $"jobListing:{request.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return new UpdateJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ModifiedAt);
    }
}
