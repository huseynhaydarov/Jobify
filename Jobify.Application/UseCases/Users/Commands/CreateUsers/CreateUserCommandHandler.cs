using AutoMapper;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Extensions.UserExtensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Domain.Common.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateUserCommandHandler : BaseSetting,  IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IPasswordHasherService _hasherService;
    
    public CreateUserCommandHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        
        user.PasswordHash = await _hasherService.HashPasswordAsync(request.Password);

        await _dbContext.Users.AddAsync(user, cancellationToken);

        user.IsActive = true;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var jobSeekerRole = await _dbContext.Roles
            .FirstAsync(r => r.Name == UserRoleNames.JobSeeker, cancellationToken);

        if (jobSeekerRole == null)
        {
            throw new NotFoundException(nameof(Role), UserRoleNames.JobSeeker);
        }

        var userRole = new UserRole()
        {
            RoleId = jobSeekerRole.Id,
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id
        };

        await _dbContext.UsersRoles.AddAsync(userRole);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}