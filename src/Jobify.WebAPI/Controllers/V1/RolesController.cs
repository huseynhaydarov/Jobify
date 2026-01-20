namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("dictionary")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<ActionResult> GetDictionary([FromQuery] GetRoleDictionaryQuery query)
    {
        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
