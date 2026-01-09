namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplicationDetail;

public class GetJobApplicationDetailQueryHandler
    : IRequestHandler<GetJobApplicationDetailQuery, GetJobApplicationDetailResponse>
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IApplicationDbContext _dbContext;

    public GetJobApplicationDetailQueryHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser)
    {
        _dbContext = dbContext;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<GetJobApplicationDetailResponse> Handle(GetJobApplicationDetailQuery request,
        CancellationToken cancellationToken)
    {
        JobApplication? application = await _dbContext.JobApplications
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
