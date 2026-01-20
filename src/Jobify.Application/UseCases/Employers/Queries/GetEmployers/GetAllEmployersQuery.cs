using Jobify.Application.Common.Models.Pagination;
using MediatR;

namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public record GetAllEmployersQuery(PagingParameters PagingParameters)
    : IRequest<PaginatedResult<GetAllEmployersResponse>>;
