namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobSeekers : ControllerBase
{
    private readonly IMediator _mediator;

    public JobSeekers(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSeekerCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }
}
