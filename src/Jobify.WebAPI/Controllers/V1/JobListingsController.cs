using Jobify.Application.UseCases.JobListings.Dtos;
using Jobify.Application.UseCases.JobListings.Queries.SearchJobListings;

namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobListingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Create(CreateJobListingCommand command)
    {
        JobListingDto data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Update([FromBody] UpdateJobListingCommand command)
    {
        UpdateJobListingResponse data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<ActionResult<JobListingDetailResponse>> GetDetail(Guid id)
    {
        JobListingDetailResponse data = await _mediator.Send(new GetJobListingDetailQuery(id));

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        GetAllJobListingsQuery query = new(parameters);

        PaginatedResult<GetAllJobListingsResponse> data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<PaginatedResult<GetAllJobListingsResponse>> Search([FromQuery] SearchRequestModel request)
    {
        var query = new SearchJobListingsQuery(
            request.SearchTerm,
            request.PageNumber,
            request.PageSize);

        return await _mediator.Send(query);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        CloseJobListingResponse data = await _mediator.Send(new DeleteJobListingCommand(id));

        return Ok(data);
    }
}
