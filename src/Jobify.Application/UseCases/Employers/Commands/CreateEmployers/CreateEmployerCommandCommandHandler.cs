using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.Employers.Dtos;
using Jobify.Domain.Constants;
using Jobify.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Employers.Commands.CreateEmployers;

public class CreateEmployerCommandCommandHandler : BaseSetting, IRequestHandler<CreateEmployerCommand, EmployerDto>
{
    private readonly IPasswordHasherService _hasherService;

    public CreateEmployerCommandCommandHandler(
        IApplicationDbContext dbContext,
        IPasswordHasherService hasherService,
        IMediator mediator,
        IAuthenticatedUserService authenticatedUserService) : base(dbContext) =>
        _hasherService = hasherService;

    public async Task<EmployerDto> Handle(CreateEmployerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            throw new DomainException("The email address is already in use.");
        }

        var role = await _dbContext.Roles
            .Where(x => x.Name == UserRoles.Employer)
            .FirstAsync(cancellationToken);


        if (role == null)
        {
            throw new NotFoundException(nameof(Role));
        }

        User user = new() { Email = request.Email, PasswordHash = request.Password, IsActive = true };

        user.PasswordHash = await _hasherService.HashPasswordAsync(request.Password);

        await _dbContext.Users.AddAsync(user, cancellationToken);

        UserRole userRole = new() { RoleId = role.Id, UserId = user.Id };

        user.UserRoles.Add(userRole);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EmployerDto(user.Id);
    }
}
