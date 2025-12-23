namespace Jobify.Application.UseCases.Employers.Queries.GetJobListingsByEmployer;

public record GetAllJobListingsByEmployerQuery(PagingParameters Parameters)
    : IRequest<PaginatedResult<GetAllJobListingsByEmployerResponse>>;
