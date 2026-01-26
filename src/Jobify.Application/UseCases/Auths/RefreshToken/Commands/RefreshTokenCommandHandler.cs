using System;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.Auths.AuthDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Jobify.Application.UseCases.Auths.RefreshToken.Commands;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService, IApplicationDbContext dbContext)
    {
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new BadRequestException("Missing refresh token.");
        }

        if (string.IsNullOrEmpty(request.AccessToken))
        {
            throw new BadRequestException("Missing access token.");
        }

        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (principal == null)
        {
            throw new UnauthorizedAccessException("Invalid access token.");
        }

        var userIdString = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new NotFoundException("User not found.", $"with id {userIdString}");
        }

        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found.", $"with id {userId}");
        }

        var newAccessToken = _tokenService.GenerateJwtToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        return new RefreshTokenResult { Token = newAccessToken, RefreshToken = newRefreshToken };
    }
}
