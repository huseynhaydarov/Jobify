using Jobify.Application.UseCases.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dictionary")]
    public async Task<ActionResult> GetDictionary([FromQuery] GetRoleDictionaryQuery query)
    {
        var roles = await _mediator.Send(query);

        return Ok(roles);
    }
}
