namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandHandler : BaseSetting, IRequestHandler<DeleteJobListingCommand, Unit>
{
    private readonly IAuthenticatedUser _authenticatedUser;

    public DeleteJobListingCommandHandler(IMapper mapper, IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser) : base(mapper, dbContext)
    {
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Unit> Handle(DeleteJobListingCommand request, CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                         ?? throw new NotFoundException("JobListing not found", request.Id);

        jobListing.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
