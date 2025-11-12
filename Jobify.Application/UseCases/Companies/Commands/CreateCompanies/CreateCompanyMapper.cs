namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyMapper : Profile
{
    public  CreateCompanyMapper()
    {
        CreateMap<CreateCompanyCommand, Company>();
    }
}
