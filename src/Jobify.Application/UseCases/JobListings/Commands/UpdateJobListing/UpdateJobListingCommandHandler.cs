using Jobify.Contracts.JobListings.Events;
using MassTransit;

namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting,
    IRequestHandler<UpdateJobListingCommand, UpdateJobListingResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateJobListingCommandHandler> _logger;
    private readonly IAuthenticatedUserService _authenticatedUserService;
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
        JobListing jobListing = await _dbContext.JobListings
                                    .Where(c => c.Id == request.Id &&
                                                c.CreatedBy == _authenticatedUserService.Id)
                                    .FirstOrDefaultAsync(cancellationToken)
                                ?? throw new NotFoundException("JobListing not found");

        jobListing.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new JobListingUpdatedEvent
        {
            Id = jobListing.Id,
            Name = jobListing.Name,
            Description = jobListing.Description,
            Requirements = jobListing.Requirements,
            Location = jobListing.Location,
            Salary =  jobListing.Salary,
            Status = jobListing.Status.ToString(),
            Currency = jobListing.Currency,
            ExpireDate = jobListing.ExpiresAt
        }, cancellationToken);

        return new UpdateJobListingResponse(
            jobListing.Id,
            jobListing.Status,
            jobListing.ModifiedAt);
    }
}
