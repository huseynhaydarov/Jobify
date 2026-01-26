using Jobify.Domain.Entities;

namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public static class JobListingMappingExtensions
{
    public static void MapFrom(this JobListing jobListing, UpdateJobListingCommand request)
    {
        jobListing.Name = request.Name;
        jobListing.Description = request.Description;
        jobListing.Requirements = request.Requirements;
        jobListing.Location = request.Location;
        jobListing.Salary = request.Salary;
        jobListing.Currency = request.Currency;
        jobListing.Status = request.Status;
    }
}
