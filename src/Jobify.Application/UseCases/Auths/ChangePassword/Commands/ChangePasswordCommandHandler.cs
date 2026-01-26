using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Auths.ChangePassword.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IApplicationDbContext _dnContext;
    private readonly IPasswordHasherService _passwordHasher;

    public ChangePasswordCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        IPasswordHasherService passwordHasher)
    {
        _dnContext = dbContext;
        _authenticatedUserService = authenticatedUserService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dnContext.Users
            .Where(c => c.Id == _authenticatedUserService.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (!await _passwordHasher.VerifyHashedPasswordAsync(request.CurrentPassword, user.PasswordHash))
        {
            throw new DomainException("Current password is incorrect");
        }

        user.PasswordHash = await _passwordHasher.HashPasswordAsync(request.NewPassword);

        await _dnContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
