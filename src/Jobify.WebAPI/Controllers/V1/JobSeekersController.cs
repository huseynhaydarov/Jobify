using Jobify.Application.UseCases.JobSeekers.Dtos;

namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobSeekersController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobSeekersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSeekerCommand command)
    {
        JobSeekerDto data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        GetAllJobSeekersQuery query = new(parameters);

        PaginatedResult<GetAllJobSeekersResponse> data = await _mediator.Send(query);

        return Ok(data);
    }
}
