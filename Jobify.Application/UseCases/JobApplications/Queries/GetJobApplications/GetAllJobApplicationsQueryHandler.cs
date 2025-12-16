namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public class GetAllJobApplicationsQueryHandler
    : IRequestHandler<GetAllJobApplicationsQuery, PaginatedList<GetAllJobApplicationsResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllJobApplicationsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<GetAllJobApplicationsResponse>> Handle(
        GetAllJobApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.JobApplications
            .AsNoTracking()
            .Include(x => x.JobListing)
            .Include(x => x.User)
            .AsQueryable();

        if (request.JobListingId.HasValue)
        {
            query = query.Where(x => x.JobListingId == request.JobListingId.Value);
        }

        var projectedQuery = query
            .OrderByDescending(x => x.AppliedAt)
            .Select(x => new GetAllJobApplicationsResponse
            {
                JobListingId = x.Id,
                JobTitle = x.JobListing!.Name,
                CoverLetter = x.CoverLetter,
                ApplicationStatus = x.ApplicationStatus,
                AppliedAt = x.AppliedAt,
                WithdrawnAt = x.WithdrawnAt
            });

        return await PaginatedList<GetAllJobApplicationsResponse>.CreateAsync(
            projectedQuery,
            request.Parameters.PageNumber,
            request.Parameters.PageSize, cancellationToken);
    }
}
