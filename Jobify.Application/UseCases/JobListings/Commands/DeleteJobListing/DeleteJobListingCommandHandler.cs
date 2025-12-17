namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting, IRequestHandler<DeleteJobListingCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteJobListingCommandHandler( IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(DeleteJobListingCommand request, CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                         ?? throw new NotFoundException("JobListing not found");

        jobListing.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
