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

    [HttpPost("create")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Create(CreateJobListingCommand command)
    {
        var data =  await _mediator.Send(command);

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobListingDetailViewModel>> GetById(Guid id)
    {
        var data = await _mediator.Send(new GetJobListingDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        var query = new GetAllJobListingsQuery(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobListingCommand(id));

        return NoContent();
    }
}
