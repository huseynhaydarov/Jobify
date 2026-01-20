namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class UserProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> Create([FromBody] CreateUserProfileCommand command)
    {
        var userProfile = await _mediator.Send(command);

        return Ok(userProfile);
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> Update([FromBody] UpdateUserProfileCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> GetDetail([FromRoute] Guid id)
    {
        var data = await _mediator.Send(new GetUserProfileDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        GetAllUserProfilesQuery query = new(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoles.Administrator)]
    [Authorize(Policy = Policies.CanPurge)]
    public async Task<IActionResult> Delete([FromBody] DeleteUserProfilesCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteUserProfileCommand(id));

        return NoContent();
    }
}
