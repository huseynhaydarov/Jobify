namespace Jobify.Application.UseCases.Roles.Queries.GetRoles;

public class GetRoleDictionaryMapper : Profile
{
    public GetRoleDictionaryMapper()
    {
        CreateMap<Role, GetRoleDictionaryViewModel>();
    }
}
