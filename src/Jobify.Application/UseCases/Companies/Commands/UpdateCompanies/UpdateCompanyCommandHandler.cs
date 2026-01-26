using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.Companies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

public class UpdateCompanyCommandHandler : BaseSetting, IRequestHandler<UpdateCompanyCommand, UpdateCompanyResponse>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IApplicationDbContext _dbContext;

    public UpdateCompanyCommandHandler(IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext)
    {
        _dbContext = dbContext;
        _authenticatedUserService = authenticatedUserService;
    }

    public async Task<UpdateCompanyResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
                          .FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedById == _authenticatedUserService.Id,
                              cancellationToken)
                      ?? throw new NotFoundException("Company not found");

        company.MapFrom(request);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateCompanyResponse(
            company.Id,
            company.ModifiedAt);
    }
}
