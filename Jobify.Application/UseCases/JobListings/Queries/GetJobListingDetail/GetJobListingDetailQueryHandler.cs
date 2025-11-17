namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public class GetJobListingByIdQueryHandler : BaseSetting, IRequestHandler<GetJobListingDetailQuery, JobListingDetailViewModel>
{

    public GetJobListingByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(mapper, dbContext)
    {
    }

    public async Task<JobListingDetailViewModel> Handle(GetJobListingDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.JobListings
                   .Where(j => j.Id == request.Id)
                   .ProjectTo<JobListingDetailViewModel>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("JobListing not found");
    }
}

