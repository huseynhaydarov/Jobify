namespace Jobify.Application.UseCases.Companies.Queries.GetCompanies;

public record GetAllCompaniesQuery(PagingParameters Parameters) : IRequest<PaginatedList<GetAllCompaniesViewModel>>;
