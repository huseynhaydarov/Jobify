using Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;

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
    public async Task<IActionResult> Create([FromBody] CreateUserProfileCommand command)
    {
        var userProfile = await _mediator.Send(command);

        return Ok(userProfile);
    }
}
