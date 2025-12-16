namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateJobSeekerCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt
                => opt.MapFrom(src => src.Password));
    }
}
