using Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;
using Jobify.Application.UseCases.UserProfiles.Command.UpdateUserProfile;
using Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;
using Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

namespace Jobify.API.Controllers.V1;


[Route("api/[controller]")]
[ApiController]
public class UserProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> Create([FromBody] CreateUserProfileCommand command)
    {
        var userProfile = await _mediator.Send(command);

        return Ok(userProfile);
    }

    [HttpPost("update")]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> Update([FromBody] UpdateUserProfileCommand command)
    {
        var userProfile = await _mediator.Send(command);

        return Ok(userProfile);
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
        var query = new GetAllUserProfilesQuery(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
