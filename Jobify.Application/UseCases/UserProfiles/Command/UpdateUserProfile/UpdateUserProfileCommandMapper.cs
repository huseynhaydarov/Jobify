using Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public class UpdateUserProfileCommandMapper : Profile
{
    public UpdateUserProfileCommandMapper()
    {
        CreateMap<UpdateUserProfileCommand, UserProfile>();
    }

}
