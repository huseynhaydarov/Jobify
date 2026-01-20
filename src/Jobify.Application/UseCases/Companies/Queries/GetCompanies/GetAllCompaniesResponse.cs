using System;

namespace Jobify.Application.UseCases.Companies.Queries.GetCompanies;

public class GetAllCompaniesResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }
}
