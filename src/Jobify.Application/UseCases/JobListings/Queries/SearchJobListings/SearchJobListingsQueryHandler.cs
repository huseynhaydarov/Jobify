using Jobify.Application.UseCases.JobListings.Queries.GetJobListings;
using SearchService.Contracts.Interfaces;
using SearchService.Contracts.Requests;

namespace Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

public class SearchJobListingsQueryHandler
    : IRequestHandler<SearchJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ISearchService _searchService;

    public SearchJobListingsQueryHandler(
        IApplicationDbContext context, IConfiguration configuration, ISearchService searchService)
    {
        _context = context;
        _searchService = searchService;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(
        SearchJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _searchService.SearchAsync(new SearchRequest
        {
            SearchTerm = request.SearchTerm
        });

        if (response.Ids.Count == 0)
        {
            return new PaginatedResult<GetAllJobListingsResponse>(
                items: new List<GetAllJobListingsResponse>(),
                request.PageNumber,
                request.PageSize,
                hasNext: false
            );
        }

        var queryable = _context.JobListings
            .Where(c => !c.IsDeleted && response.Ids.Contains(c.Id))
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
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
