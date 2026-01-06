namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly ILogger<GetAllJobListingsQueryHandler> _logger;

    public GetAllJobListingsQueryHandler(
        IApplicationDbContext dbContext,
        ILogger<GetAllJobListingsQueryHandler> logger)
        : base(dbContext)
    {
        _logger = logger;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(GetAllJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.JobListings
            .Where(c => !c.IsDeleted && c.Status == JobStatus.Open)
            .AsNoTracking()
            .Select(c => new GetAllJobListingsResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Requirements = c.Requirements,
                Location = c.Location,
                Salary = c.Salary,
                Currency = c.Currency,
                Status = c.Status,
                PostedAt = c.PostedAt,
                Views = c.Views,
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}

