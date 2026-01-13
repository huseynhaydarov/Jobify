namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public class CreateJobSeekerCommandHandler : BaseSetting, IRequestHandler<CreateJobSeekerCommand, JobSeekerDto>
{
    private readonly IPasswordHasherService _hasherService;

    public CreateJobSeekerCommandHandler(
        IApplicationDbContext dbContext,
        IPasswordHasherService hasherService,
        IMediator mediator) : base(dbContext) =>
        _hasherService = hasherService;

    public async Task<JobSeekerDto> Handle(CreateJobSeekerCommand request, CancellationToken cancellationToken)
    {
        User? existingUser = await _dbContext.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            throw new DomainException("The email address is already in use.");
        }

        Role? role = await _dbContext.Roles
            .Where(x => x.Name == UserRoles.JobSeeker)
            .FirstAsync(cancellationToken);

        if (role == null)
        {
            throw new NotFoundException(nameof(Role));
        }

        User user = new()
        {
            Email = request.Email, PasswordHash = request.Password, IsActive = true
        };

        user.PasswordHash = await _hasherService.HashPasswordAsync(request.Password);

        await _dbContext.Users.AddAsync(user, cancellationToken);

        UserRole userRole = new() { RoleId = role.Id, UserId = user.Id };

        user.UserRoles.Add(userRole);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new JobSeekerDto(user.Id);
    }
}
