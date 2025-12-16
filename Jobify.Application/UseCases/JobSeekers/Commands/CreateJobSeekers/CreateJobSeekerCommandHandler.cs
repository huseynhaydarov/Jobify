namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public class CreateJobSeekerCommandHandler : BaseSetting,  IRequestHandler<CreateJobSeekerCommand, JobSeekerDto>
{
    private readonly IPasswordHasherService _hasherService;
    private readonly IMediator _mediator;

    public CreateJobSeekerCommandHandler(
        IMapper mapper,
        IApplicationDbContext dbContext,
        IPasswordHasherService hasherService,
        IMediator mediator) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
        _mediator = mediator;
    }

    public async Task<JobSeekerDto> Handle(CreateJobSeekerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            throw new DomainException("The email address is already in use.");
        }

        var role = await _dbContext.Roles
            .Where(x => x.Name == UserRoles.JobSeeker)
            .FirstAsync(cancellationToken);

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

        return new JobSeekerDto(user.Id);
    }
}
