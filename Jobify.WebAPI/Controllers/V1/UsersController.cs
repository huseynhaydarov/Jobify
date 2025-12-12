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

    [HttpPost("create-jobseeker")]
    public async Task<IActionResult> Create([FromBody] CreateJobSeekerCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPost("create-employer")]
    public async Task<IActionResult> Create([FromBody] CreateEmployerCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }
}
