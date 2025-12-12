namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _mediator;

    public CompaniesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        var data =  await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPost("update")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Update([FromBody] UpdateCompanyCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<ActionResult<GetCompanyDetailResponse>> GetDetail([FromRoute] Guid id)
    {
        var data = await _mediator.Send(new GetCompanyDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters, CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery(parameters);

        var data = await _mediator.Send(query, cancellationToken);

        return Ok(data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Administrator)]
    [Authorize(Policy = Policies.CanPurge)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteJobListingCommand(id));

        return NoContent();
    }
}

