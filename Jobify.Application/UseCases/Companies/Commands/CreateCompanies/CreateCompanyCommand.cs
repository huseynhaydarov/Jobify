namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public record CreateCompanyCommand(
    string Name,
    string? Description,
    string? WebsiteUrl,
    string? Industry) : IRequest<Unit>;
