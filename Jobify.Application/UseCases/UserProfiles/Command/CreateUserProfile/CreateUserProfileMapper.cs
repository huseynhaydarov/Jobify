namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileMapper : Profile
{
    public CreateUserProfileMapper()
    {
        CreateMap<CreateUserProfileCommand, UserProfile>();
    }
}
