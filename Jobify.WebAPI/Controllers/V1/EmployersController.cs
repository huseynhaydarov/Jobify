using Jobify.Application.UseCases.Employers.Commands.PositionUpdate;

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

    [HttpPost("join-company")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> JoinCompany([FromBody] JoinCompanyCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpPost("position-update")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> Update([FromBody] PositionUpdateCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }
}
