namespace Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;

public static class UserProfileMappingExtensions
{
    public static void MapTo(this UserProfile userProfile, UpdateUserProfileCommand request)
    {
        userProfile.FirstName = request.FirstName;
        userProfile.LastName = request.LastName;
        userProfile.PhoneNumber = request.PhoneNumber;
        userProfile.Location =  request.Location;
        userProfile.Bio = request.Bio;
        userProfile.Education = request.Education;
        userProfile.Experience = request.Experience;
    }
}
