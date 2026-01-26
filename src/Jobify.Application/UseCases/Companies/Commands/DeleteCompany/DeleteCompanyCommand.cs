using System;
using MediatR;

namespace Jobify.Application.UseCases.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(Guid CompanyId) : IRequest<Unit>;
