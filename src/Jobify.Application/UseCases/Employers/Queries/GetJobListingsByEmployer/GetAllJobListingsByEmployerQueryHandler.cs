using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Employers.Queries.GetJobListingsByEmployer;

public class GetAllJobListingsByEmployerQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsByEmployerQuery, PaginatedResult<GetAllJobListingsByEmployerResponse>>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public GetAllJobListingsByEmployerQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService)
        : base(dbContext) =>
        _authenticatedUserService = authenticatedUserService;

    public async Task<PaginatedResult<GetAllJobListingsByEmployerResponse>> Handle(
        GetAllJobListingsByEmployerQuery request,
        CancellationToken cancellationToken)
    {
        var employerCompanyId = await _dbContext.Employers
            .AsNoTracking()
            .Where(c => c.UserId == _authenticatedUserService.Id)
            .Select(e => e.CompanyId)
            .FirstOrDefaultAsync(cancellationToken);

        var queryable = _dbContext.JobListings
            .IgnoreQueryFilters()
            .Where(c => c.CompanyId == employerCompanyId && c.CreatedBy == _authenticatedUserService.Id)
            .AsNoTracking()
            .Select(c => new GetAllJobListingsByEmployerResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Requirements = c.Requirements,
                Location = c.Location,
                Salary = c.Salary,
                Currency = c.Currency,
                Status = c.Status,
                PostedAt = c.PostedAt
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
