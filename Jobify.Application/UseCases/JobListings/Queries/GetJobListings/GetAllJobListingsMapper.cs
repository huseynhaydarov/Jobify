namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsMapper : Profile
{
    public GetAllJobListingsMapper()
    {
        CreateMap<JobListing, GetAllJobListingsViewModel>();
    }
}
