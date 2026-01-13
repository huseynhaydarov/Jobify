namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public record GetAllEmployersQuery(PagingParameters PagingParameters)
    : IRequest<PaginatedResult<GetAllEmployersResponse>>;
