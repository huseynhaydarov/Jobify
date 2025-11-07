namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }

}
