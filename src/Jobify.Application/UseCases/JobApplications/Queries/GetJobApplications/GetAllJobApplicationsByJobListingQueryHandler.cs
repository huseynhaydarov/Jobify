using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public class GetAllJobApplicationsByJobListingQueryHandler
    : IRequestHandler<GetAllJobApplicationsByJobListingQuery,
        PaginatedResult<GetAllJobApplicationsByJobListingResponse>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllJobApplicationsByJobListingQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<PaginatedResult<GetAllJobApplicationsByJobListingResponse>> Handle(
        GetAllJobApplicationsByJobListingQuery request,
        CancellationToken cancellationToken)
    {
        if (!request.JobListingId.HasValue)
        {
            throw new BadRequestException("Id is required.");
        }

        var queryable = _dbContext.JobApplications
            .AsNoTracking()
            .OrderByDescending(c => c.AppliedAt)
            .Select(x => new GetAllJobApplicationsByJobListingResponse
            {
                Id = x.Id,
                JobTitle = x.JobListing!.Name,
                CoverLetter = x.CoverLetter,
                ApplicationStatus = x.ApplicationStatus,
                AppliedAt = x.AppliedAt,
                WithdrawnAt = x.WithdrawnAt
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
