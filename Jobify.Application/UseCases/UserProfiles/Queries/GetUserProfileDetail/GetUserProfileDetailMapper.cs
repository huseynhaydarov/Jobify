namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailMapper : Profile
{
    public GetUserProfileDetailMapper()
    {
        CreateMap<UserProfile, GetUserProfileDetailVievModel>();
    }
}
