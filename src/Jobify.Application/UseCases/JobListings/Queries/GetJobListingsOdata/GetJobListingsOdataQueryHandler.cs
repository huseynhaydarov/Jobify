using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.UseCases.Companies.Dtos;
using Jobify.Application.UseCases.Employers.Dtos;
using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingsOdata;

public class GetJobListingsOdataQueryHandler
    : IRequestHandler<GetAllJobListingsOdataQuery, IQueryable<JobListingOdataDto>>
{
    private readonly IApplicationDbContext _context;

    public GetJobListingsOdataQueryHandler(IApplicationDbContext context) => _context = context;

    public Task<IQueryable<JobListingOdataDto>> Handle(
        GetAllJobListingsOdataQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.JobListings
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Status == JobStatus.Open)
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
                Company = new CompanyOdataDto { Id = j.Company!.Id, Name = j.Company.Name },
                Employer = new EmployerOdataDto
                {
                    Id = j.Employer!.Id,
                    FullName = j.Employer.User.UserProfile.FirstName + " " +
                               j.Employer.User.UserProfile.LastName
                }
            });

        return Task.FromResult(query);
    }
}
