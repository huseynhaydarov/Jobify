namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public record GetAllJobSeekersQuery(PagingParameters PagingParameters)
    : IRequest<PaginatedList<GetAllJobSeekersResponse>>;

