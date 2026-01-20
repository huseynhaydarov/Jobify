using Jobify.Application.Common.Models.Pagination;
using MediatR;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public record GetAllJobListingsQuery(
    PagingParameters Parameters) : IRequest<PaginatedResult<GetAllJobListingsResponse>>;
