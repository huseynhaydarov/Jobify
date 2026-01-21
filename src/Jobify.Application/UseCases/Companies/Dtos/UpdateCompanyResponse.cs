using System;

namespace Jobify.Application.UseCases.Companies.Dtos;

public record UpdateCompanyResponse(Guid Id, DateTimeOffset? UpdatedAt);
