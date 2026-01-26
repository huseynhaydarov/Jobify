using Jobify.Application.Common.Models.Pagination;
using MediatR;

namespace Jobify.Application.UseCases.Employers.Queries.GetJobListingsByEmployer;

public record GetAllJobListingsByEmployerQuery(PagingParameters Parameters)
    : IRequest<PaginatedResult<GetAllJobListingsByEmployerResponse>>;
