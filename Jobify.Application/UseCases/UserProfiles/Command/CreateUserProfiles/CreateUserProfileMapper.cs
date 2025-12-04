namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;

public class CreateUserProfileMapper : Profile
{
    public CreateUserProfileMapper()
    {
        CreateMap<CreateUserProfileCommand, UserProfile>();
    }
}
