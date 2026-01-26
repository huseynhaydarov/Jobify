using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.Companies.Dtos;
using Jobify.Domain.Entities;
using MediatR;

namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyCommandHandler : BaseSetting, IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public CreateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext) =>
        _authenticatedUserService = authenticatedUserService;

    public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var userId = _authenticatedUserService.Id
                     ?? throw new UnauthorizedException("User is not authenticated");

        var company = new Company
        {
            Name = request.Name,
            Description = request.Description,
            WebsiteUrl = request.WebsiteUrl,
            Industry = request.Industry,
            CreatedById = userId
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCompanyResponse(
            company.Id,
            company.CreatedAt);
    }
}
