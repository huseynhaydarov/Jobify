namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandHandler : BaseSetting, IRequestHandler<UpdateJobListingCommand, Unit>
{
    public UpdateJobListingCommandHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<Unit> Handle(UpdateJobListingCommand request, CancellationToken cancellationToken)
    {
        var jobListing = await _dbContext.JobListings
                          .Where(c => c.Id == request.Id)
                          .FirstOrDefaultAsync(cancellationToken)
                      ?? throw new NotFoundException("JobListing not found");

        _mapper.Map(request, jobListing);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
