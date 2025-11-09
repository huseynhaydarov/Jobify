using Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;

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
}
