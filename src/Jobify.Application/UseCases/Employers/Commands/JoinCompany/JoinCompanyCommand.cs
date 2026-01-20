using System;
using MediatR;

namespace Jobify.Application.UseCases.Employers.Commands.JoinCompany;

public record JoinCompanyCommand(Guid CompanyId) : IRequest<Unit>;
