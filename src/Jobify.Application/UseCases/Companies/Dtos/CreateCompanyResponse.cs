using System;

namespace Jobify.Application.UseCases.Companies.Dtos;

public record CreateCompanyResponse(Guid Id, DateTimeOffset CreatedAt);
