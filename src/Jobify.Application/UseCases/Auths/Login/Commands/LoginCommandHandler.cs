using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.Auths.AuthDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Auths.Login.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context,
        ITokenService tokenService,
        IPasswordHasherService passwordHasher)
    {
        _dbContext = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("Invalid credentials");
        }

        try
        {
            if (!await _passwordHasher.VerifyHashedPasswordAsync(command.Password, user.PasswordHash))
            {
                return UnauthorizedResponse("Invalid credentials");
            }
        }
        catch (ArgumentException)
        {
            return UnauthorizedResponse("Invalid credentials");
        }

        if (!user.IsActive)
        {
            return UnauthorizedResponse("Account is not active");
        }

        var token = _tokenService.GenerateJwtToken(user);

        var refreshToken = _tokenService.GenerateRefreshToken();

        return new AuthResponse(
            true,
            token,
            refreshToken,
            user.Email,
            user.UserRoles.Select(r => r.Role?.Name).Aggregate((x, y) => x + ", " + y));
    }

    private static AuthResponse UnauthorizedResponse(string errorMessage) =>
        new(
            false,
            string.Empty,
            string.Empty,
            Errors: [errorMessage]);
}
