using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfile;

public class DeleteUserProfileCommandHandler : BaseSetting, IRequestHandler<DeleteUserProfileCommand, Unit>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DeleteUserProfileCommandHandler> _logger;


    public DeleteUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<DeleteUserProfileCommandHandler> logger,
        IDistributedCache cache) : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _dbContext.UserProfiles
                              .Where(u => u.Id == request.Id && u.UserId == _authenticatedUserService.Id)
                              .FirstOrDefaultAsync(cancellationToken)
                          ?? throw new NotFoundException("Profile not found");

        userProfile.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = $"userProfile:{request.Id}";
        _logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return Unit.Value;
    }
}
