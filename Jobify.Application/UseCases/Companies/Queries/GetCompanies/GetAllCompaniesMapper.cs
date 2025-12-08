namespace Jobify.Application.UseCases.Companies.Queries.GetCompanies;

public class GetAllCompaniesMapper : Profile
{
    public GetAllCompaniesMapper()
    {
        CreateMap<Company, GetAllCompaniesViewModel>();
    }
}
