namespace Jobify.Application.UseCases.JobListings.Commands.CreateJobListing;

public class CreateJobListingMapper : Profile
{
    public CreateJobListingMapper()
    {
        CreateMap<CreateJobListingCommand, JobListing>();
    }
}
