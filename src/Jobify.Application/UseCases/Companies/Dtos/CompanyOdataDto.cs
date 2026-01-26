using System;

namespace Jobify.Application.UseCases.Companies.Dtos;

public record CompanyOdataDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
