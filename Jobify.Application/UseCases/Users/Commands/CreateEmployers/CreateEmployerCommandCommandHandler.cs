namespace Jobify.Application.UseCases.Users.Commands.CreateEmployers;

public class CreateEmployerCommandCommandHandler : BaseSetting, IRequestHandler<CreateEmployerCommand, Guid>
{
    private readonly IPasswordHasherService _hasherService;
    private readonly IMediator _mediator;

    public CreateEmployerCommandCommandHandler(IApplicationDbContext dbContext, IPasswordHasherService hasherService, IMapper mapper,
        IMediator mediator) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateEmployerCommand request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.FirstAsync(r => r.Name == UserRoles.Employer, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException(nameof(Role));
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

        await _mediator.Publish(new EmployerCreatedEvent(user), cancellationToken);

        return user.Id;
    }

}
