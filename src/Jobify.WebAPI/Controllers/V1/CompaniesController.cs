namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _mediator;

    public CompaniesController(ISender mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Update([FromBody] UpdateCompanyCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<ActionResult<GetCompanyDetailResponse>> GetDetail([FromRoute] Guid id)
    {
        var data = await _mediator.Send(new GetCompanyDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters,
        CancellationToken cancellationToken)
    {
        GetAllCompaniesQuery query = new(parameters);

        var data = await _mediator.Send(query, cancellationToken);

        return Ok(data);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.CanPurge)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteCompanyCommand(id));

        return NoContent();
    }
}
