namespace Jobify.Application.UseCases.Roles.Queries;

public class GetRoleDictionaryMapper : Profile
{
    public GetRoleDictionaryMapper()
    {
        CreateMap<Role, GetRoleDictionaryViewModel>();
    }
}
