namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesQueryMapper : Profile
{
    public GetAllUserProfilesQueryMapper()
    {
        CreateMap<UserProfile, GetAllUserProfilesViewModel>()
            .ForMember(c => c.FullName, opt =>
                opt.MapFrom(x => $"{x.LastName} {x.FirstName}"));
    }
}
