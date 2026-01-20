namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class EmployersController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployerCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPost("join-company")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> JoinCompany([FromBody] JoinCompanyCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpPatch("{Id}/position")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] PositionUpdateCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        GetAllEmployersQuery query = new(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("job-listings")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> GetJobListingsByEmployer([FromQuery] PagingParameters parameters)
    {
        GetAllJobListingsByEmployerQuery query = new(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
