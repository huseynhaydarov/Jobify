namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingsOdata;

public class GetJobListingsOdataQueryHandler
    : IRequestHandler<GetAllJobListingsOdataQuery, IQueryable<JobListingOdataDto>>
{
    private readonly IApplicationDbContext _context;

    public GetJobListingsOdataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<IQueryable<JobListingOdataDto>> Handle(
        GetAllJobListingsOdataQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.JobListings
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(j => new JobListingOdataDto
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                Requirements = j.Requirements,
                Location = j.Location,
                Salary = j.Salary,
                Currency = j.Currency,
                Status = j.Status,
                PostedAt = j.PostedAt,
                ExpiresAt = j.ExpiresAt,
                Views = j.Views,
                Company = new CompanyOdataDto
                {
                    Id = j.Company!.Id,
                    Name = j.Company.Name
                },
                Employer = new EmployerOdataDto
                {
                    Id = j.Employer!.Id,
                    FullName = j.Employer.User.UserProfile.FirstName + " " + j.Employer.User.UserProfile.LastName
                }
            });

        return Task.FromResult(query);
    }
}
