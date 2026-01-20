using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplicationDetail;

public class GetJobApplicationDetailQueryHandler
    : IRequestHandler<GetJobApplicationDetailQuery, GetJobApplicationDetailResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IApplicationDbContext _dbContext;

    public GetJobApplicationDetailQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService)
    {
        _dbContext = dbContext;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<GetJobApplicationDetailResponse> Handle(GetJobApplicationDetailQuery request,
        CancellationToken cancellationToken)
    {
        var application = await _dbContext.JobApplications
            .AsNoTracking()
            .Include(x => x.JobListing)
            .ThenInclude(j => j.Company)
            .Include(x => x.User)
            .ThenInclude(u => u.UserProfile)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("Job application not found.");
        }

        return new GetJobApplicationDetailResponse
        {
            Id = application.Id,
            JobListingId = application.JobListingId,
            JobTitle = application.JobListing?.Name,
            CompanyName = application.JobListing?.Company?.Name,
            ApplicantId = application.ApplicantId,
            ApplicantName = application.User?.UserProfile?.FirstName,
            ApplicantEmail = application.User?.Email,
            CoverLetter = application.CoverLetter,
            ApplicationStatus = application.ApplicationStatus,
            AppliedAt = application.AppliedAt,
            WithdrawnAt = application.WithdrawnAt
        };
    }
}
