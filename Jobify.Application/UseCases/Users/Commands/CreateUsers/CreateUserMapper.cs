namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateJobSeekerCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}