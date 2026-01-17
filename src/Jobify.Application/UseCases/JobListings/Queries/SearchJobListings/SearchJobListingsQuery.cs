using Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

namespace Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

public sealed record SearchJobListingsQuery(
    string SearchTerm,
    PagingParameters Parameters)
    : IRequest<PaginatedResult<GetAllJobListingsResponse>>;
