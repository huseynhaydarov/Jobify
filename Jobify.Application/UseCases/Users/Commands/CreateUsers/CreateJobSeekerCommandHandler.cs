namespace Jobify.Application.UseCases.Users.Commands.CreateUsers;

public class CreateJobSeekerCommandHandler : BaseSetting,  IRequestHandler<CreateJobSeekerCommand, Guid>
{
    private readonly IPasswordHasherService _hasherService;
    private readonly IMediator _mediator;

    public CreateJobSeekerCommandHandler(IMapper mapper, IApplicationDbContext dbContext, IPasswordHasherService hasherService,
        IMediator mediator) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateJobSeekerCommand request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.FirstAsync(r => r.Name == UserRoles.JobSeeker, cancellationToken);

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

        return user.Id;
    }
}
