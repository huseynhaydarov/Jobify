namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public class UpdateCompanyMapper : Profile
{
    public UpdateCompanyMapper()
    {
        CreateMap<UpdateCompanyCommand, Company>();
    }
}
