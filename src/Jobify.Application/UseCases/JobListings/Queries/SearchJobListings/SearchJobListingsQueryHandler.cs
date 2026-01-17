using Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

namespace Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

public class SearchJobListingsQueryHandler
    : IRequestHandler<SearchJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly IJobSearchClientService _searchClient;
    private readonly IApplicationDbContext _context;

    public SearchJobListingsQueryHandler(
        IJobSearchClientService searchClient,
        IApplicationDbContext context)
    {
        _searchClient = searchClient;
        _context = context;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(
        SearchJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var ids = await _searchClient.SearchAsync(
            request.SearchTerm,
            cancellationToken);

        if (ids.Count == 0)
        {
            return new PaginatedResult<GetAllJobListingsResponse>(
                items: new List<GetAllJobListingsResponse>(),
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                hasNext: false
            );
        }

        var queryable = _context.JobListings
            .Where(c => !c.IsDeleted && ids.Contains(c.Id))
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
                PostedAt = c.PostedAt
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
