using System;

namespace Jobify.Application.UseCases.Employers.Dtos;

public record EmployerOdataDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
}
