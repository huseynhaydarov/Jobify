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
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                         ?? throw new NotFoundException("JobListing not found", request.Id);

        bool isEmployerOwner = jobListing.EmployerId == _authenticatedUser.Id;

        if (!isEmployerOwner)
        {
            throw new ForbiddenAccessException("Don't have access to delete the current job listing.");
        }

        _dbContext.JobListings.Remove(jobListing);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
