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
            .FirstOrDefaultAsync(x => x.Id == request.Id &&
                                      x.Employer != null &&
                                      x.Employer.UserId == _authenticatedUser.Id, cancellationToken)
                         ?? throw new NotFoundException("JobListing not found", request.Id);


        jobListing.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
