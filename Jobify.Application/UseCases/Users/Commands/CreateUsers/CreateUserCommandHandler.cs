using Jobify.Domain.Events;

namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateUserCommandHandler : BaseSetting,  IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IPasswordHasherService _hasherService;
    private readonly IMediator _mediator;

    public CreateUserCommandHandler(IMapper mapper, IApplicationDbContext dbContext, IPasswordHasherService hasherService,
        IMediator mediator) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
        _mediator = mediator;
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

        if (role.Name == UserRoles.Employer)
        {
            await _mediator.Publish(new EmployerCreatedEvent(user), cancellationToken);
        }

        return user.Id;
    }
}
