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

        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (principal == null)
        {
            throw new UnauthorizedAccessException("Invalid access token.");
        }

        string? userIdString = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
        {
            throw new NotFoundException("User not found.", $"with id {userIdString}");
        }

        User? user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found.", $"with id {userId}");
        }

        string newAccessToken = _tokenService.GenerateJwtToken(user);
        string newRefreshToken = _tokenService.GenerateRefreshToken();

        return new RefreshTokenResult { Token = newAccessToken, RefreshToken = newRefreshToken };
    }
}
