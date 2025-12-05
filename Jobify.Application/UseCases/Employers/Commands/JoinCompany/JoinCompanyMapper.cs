namespace Jobify.Application.UseCases.Employers.Commands.JoinCompany;

public class JoinCompanyMapper : Profile
{
    public JoinCompanyMapper()
    {
        CreateMap<JoinCompanyCommand, Employer>();
    }
}
