namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandMapper : Profile
{
    public UpdateJobListingCommandMapper()
    {
        CreateMap<UpdateJobListingCommand, JobListing>();
    }
}
