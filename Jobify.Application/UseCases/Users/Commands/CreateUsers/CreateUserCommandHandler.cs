using AutoMapper;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Interfaces.Services;
using Jobify.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateUserCommandHandler : BaseSetting,  IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IPasswordHasherService _hasherService;

    public CreateUserCommandHandler(IMapper mapper, IApplicationDbContext dbContext, IPasswordHasherService hasherService) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException(nameof(Role), request.RoleId);
        }

        var user = _mapper.Map<User>(request);

        user.IsActive = true;

        user.PasswordHash = await _hasherService.HashPasswordAsync(request.Password);

        await _dbContext.Users.AddAsync(user, cancellationToken);

        var userRole = new UserRole()
        {
            RoleId = role.Id,
            UserId = user.Id
        };

        user.UserRoles.Add(userRole);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
