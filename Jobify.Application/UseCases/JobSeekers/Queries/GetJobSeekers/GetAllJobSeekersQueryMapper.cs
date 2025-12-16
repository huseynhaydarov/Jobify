namespace Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;

public class GetAllJobSeekersQueryMapper : Profile
{
    public GetAllJobSeekersQueryMapper()
    {
        CreateMap<User, GetAllJobSeekersResponse>();
    }
}
