using System;

namespace Jobify.Application.UseCases.Companies.Dtos;

public record UpdateCompanyRequest
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? WebsiteUrl { get; init; }
    public string? Description { get; init; }
    public string? Industry { get; init; }
}
