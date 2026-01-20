using Jobify.Application.UseCases.JobListings.Queries.GetJobListings;
using SearchService.Contracts.Requests;

namespace Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

public sealed record SearchJobListingsQuery(
   SearchRequest Request, PagingParameters Parameters)
    : IRequest<PaginatedResult<GetAllJobListingsResponse>>;
