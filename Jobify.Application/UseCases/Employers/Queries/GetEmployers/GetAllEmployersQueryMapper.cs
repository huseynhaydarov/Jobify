namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public class GetAllEmployersQueryMapper : Profile
{
    public GetAllEmployersQueryMapper()
    {
        CreateMap<User, GetAllEmployersResponse>();
    }
}
