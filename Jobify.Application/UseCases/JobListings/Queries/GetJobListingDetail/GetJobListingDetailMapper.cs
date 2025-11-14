namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public class GetJobListingDetailMapper : Profile
{
    public GetJobListingDetailMapper()
    {
        CreateMap<JobListing, JobListingDetailViewModel>();
    }
}
