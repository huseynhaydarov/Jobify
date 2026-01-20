using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Application.UseCases.UserProfiles.Dtos;
using Jobify.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandHandler : BaseSetting, IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;

    public CreateUserProfileCommandHandler(
        IApplicationDbContext dbContext,
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CreateUserProfileCommandHandler> logger)
        : base(dbContext)
    {
        _authenticatedUserService = authenticatedUserService;
        _logger = logger;
    }

    public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _authenticatedUserService.Id
                     ?? throw new UnauthorizedException("User is not authenticated");

        UserProfile userProfile = new()
        {
            UserId = userId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            Bio = request.Bio,
            Education = request.Education,
            Experience = request.Experience
        };

        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserProfileDto(userProfile.Id);
    }
}
