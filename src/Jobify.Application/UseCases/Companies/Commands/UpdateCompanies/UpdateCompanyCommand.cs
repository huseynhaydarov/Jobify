namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public record UpdateCompanyCommand(
    Guid Id,
    string Name,
    string? WebsiteUrl,
    string? Description,
    string? Industry) : IRequest<Unit>;
