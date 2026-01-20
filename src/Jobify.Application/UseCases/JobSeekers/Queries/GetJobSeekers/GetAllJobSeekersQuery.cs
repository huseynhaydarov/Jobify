using Jobify.Application.Common.Models.Pagination;
using MediatR;

namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public record GetAllJobSeekersQuery(PagingParameters PagingParameters)
    : IRequest<PaginatedResult<GetAllJobSeekersResponse>>;
