namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public static class CompanyMappingExtensions
{
    public static void MapFrom(this Company company, UpdateCompanyCommand request)
    {
        company.Name = request.Name;
        company.WebsiteUrl = request.WebsiteUrl;
        company.Description = request.Description;
        company.Industry = request.Industry;
    }
}
