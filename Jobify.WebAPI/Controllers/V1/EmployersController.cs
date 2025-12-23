using Jobify.Application.UseCases.Employers.Queries.GetJobListingsByEmployer;

namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class EmployersController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployersController(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    [HttpPut("position")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Update([FromBody] PositionUpdateCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        var query = new GetAllEmployersQuery(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("job-listings")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> GetJobListingsByEmployer([FromQuery] PagingParameters parameters)
    {
        var query = new GetAllJobListingsByEmployerQuery(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
