namespace Jobify.Application.UseCases.Auths.ChangePassword.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IApplicationDbContext _dnContext;
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IPasswordHasherService _passwordHasher;

    public ChangePasswordCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUser authenticatedUser,
        IPasswordHasherService passwordHasher)
    {
        _dnContext = dbContext;
        _authenticatedUser = authenticatedUser;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dnContext.Users
            .Where(c => c.Id == _authenticatedUser.Id)
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
