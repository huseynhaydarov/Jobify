using Jobify.Application.UseCases.JobListings.Queries.GetJobListings;
using Refit;
using SearchService.ApiClient;

namespace Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

public class SearchJobListingsQueryHandler
    : IRequestHandler<SearchJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public SearchJobListingsQueryHandler(
        IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(
        SearchJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var searchApi = RestService.For<IJobSearchApi>(_configuration["SearchService:BaseUrl"]);

        var response = await searchApi.SearchAsync(request.Request, cancellationToken);

        if (response.Ids.Count == 0)
        {
            return new PaginatedResult<GetAllJobListingsResponse>(
                items: new List<GetAllJobListingsResponse>(),
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
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
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
