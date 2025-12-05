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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobListingCommand(id));

        return NoContent();
    }
}
