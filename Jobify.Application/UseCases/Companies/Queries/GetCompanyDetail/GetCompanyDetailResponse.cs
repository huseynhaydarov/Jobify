namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailResponse
{
    public required string Name { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }
}

