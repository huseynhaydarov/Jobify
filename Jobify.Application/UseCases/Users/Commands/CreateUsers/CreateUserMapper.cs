using AutoMapper;
using Jobify.Domain.Common.Entities;

namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}