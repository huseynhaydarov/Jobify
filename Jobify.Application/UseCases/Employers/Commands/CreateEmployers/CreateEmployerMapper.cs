namespace Jobify.Application.UseCases.Employers.Commands.CreateEmployers;

public class CreateEmployerMapper : Profile
{
    public CreateEmployerMapper()
    {
        CreateMap<CreateEmployerCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}
