namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ISender _mediator;

    public CompanyController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("update")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Update([FromBody] UpdateCompanyCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<GetCompanyDetailViewModel>> GetDetail([FromRoute] Guid id)
    {
        var data = await _mediator.Send(new GetCompanyDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters, CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery(parameters);

        var data = await _mediator.Send(query, cancellationToken);

        return Ok(data);
    }

}

