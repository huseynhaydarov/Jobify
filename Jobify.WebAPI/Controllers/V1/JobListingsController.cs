namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Create(CreateJobListingCommand command)
    {
        var data =  await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Update([FromBody] UpdateJobListingCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<JobListingDetailResponse>> GetDetail(Guid id)
    {
        var data = await _mediator.Send(new GetJobListingDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        var query = new GetAllJobListingsQuery(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteJobListingCommand(id));

        return NoContent();
    }
}
