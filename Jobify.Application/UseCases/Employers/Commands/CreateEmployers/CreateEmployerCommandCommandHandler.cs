namespace Jobify.Application.UseCases.Employers.Commands.CreateEmployers;

public class CreateEmployerCommandCommandHandler : BaseSetting, IRequestHandler<CreateEmployerCommand, EmployerDto>
{
    private readonly IPasswordHasherService _hasherService;

    public CreateEmployerCommandCommandHandler(
        IApplicationDbContext dbContext,
        IPasswordHasherService hasherService,
        IMapper mapper,
        IMediator mediator) : base(mapper, dbContext)
    {
        _hasherService = hasherService;
    }

    public async Task<EmployerDto> Handle(CreateEmployerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser != null)
        {
            throw new DomainException("The email address is already in use.");
        }

        var role = await _dbContext.Roles
            .Where(x => x.Name == UserRoles.Employer)
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

        return new EmployerDto(user.Id);
    }
}
