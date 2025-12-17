namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public class GetJobListingByIdQueryHandler : BaseSetting,
    IRequestHandler<GetJobListingDetailQuery, JobListingDetailResponse>
{

    public GetJobListingByIdQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<JobListingDetailResponse> Handle(GetJobListingDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.JobListings
                   .Where(j => j.Id == request.Id)
                   .Select(c => new JobListingDetailResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Salary = c.Salary,
                        Currency = c.Currency,
                        Location = c.Location,
                        Status = c.Status,
                        PostedAt = c.PostedAt,
                        Requirements = c.Requirements,
                        CompanyName = c.Company.Name,
                        EmployerEmail = c.Employer.User.Email,
                    })
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("JobListing not found");
    }
}

