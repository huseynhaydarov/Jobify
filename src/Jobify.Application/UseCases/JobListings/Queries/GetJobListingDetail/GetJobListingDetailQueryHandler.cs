namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public class GetJobListingByIdQueryHandler : BaseSetting,
    IRequestHandler<GetJobListingDetailQuery, JobListingDetailResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetJobListingByIdQueryHandler> _logger;


    public GetJobListingByIdQueryHandler(
        IDistributedCache cache,
        IApplicationDbContext dbContext,
        ILogger<GetJobListingByIdQueryHandler> logger) : base(dbContext)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<JobListingDetailResponse> Handle(GetJobListingDetailQuery request,
        CancellationToken cancellationToken)
    {
        string cacheKey = $"jobListing:{request.Id}";
        _logger.LogInformation("fetching data for key: {CacheKey} from cache.", cacheKey);

         JobListingDetailResponse? jobListing = await _cache.GetOrSetAsync(cacheKey,
            async () =>
            {
                _logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database.", cacheKey);
                return await _dbContext.JobListings
                           .AsNoTracking()
                           .Where(j => j.Id == request.Id)
                           .Select(c => new JobListingDetailResponse
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Description = c.Description,
                               Salary = c.Salary,
                               Currency = c.Currency,
                               Location = c.Location,
                               Status = c.Status,
                               PostedAt = c.PostedAt,
                               Requirements = c.Requirements,
                               CompanyName = c.Company.Name,
                               EmployerEmail = c.Employer.User.Email
                           })
                           .FirstOrDefaultAsync(cancellationToken)
                       ?? throw new NotFoundException("JobListing not found");
            });

        if (jobListing == null)
        {
            throw new NullDataException("Retrieved data is null");
        }

        return jobListing;
    }
}
