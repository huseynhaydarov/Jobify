using Jobify.Application.UseCases.JobApplications.Commands.UpdateJobApplicationStatus;

namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("apply")]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<IActionResult> Apply([FromBody] CreateJobApplicationCommand command)
    {
        var application = await _mediator.Send(command);

        return Ok(application);
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelJobApplicationCommand command)
    {
        var application = await _mediator.Send(command);

        return Ok(application);
    }

    [HttpGet("jobListing")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<GetAllJobApplicationsByJobListingResponse>> GetAll(
        [FromQuery] Guid? jobListingId,
        [FromQuery] PagingParameters paging)
    {
        var result = await _mediator.Send(new GetAllJobApplicationsByJobListingQuery(jobListingId, paging));

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetJobApplicationDetailResponse>> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetJobApplicationDetailQuery(id));

        return Ok(result);
    }

    [HttpPatch("{Id}/status")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Update([FromRoute] Guid Id,  [FromBody] ApplicationStatusUpdateCommand command)
    {
        if (Id != command.applicationId)
        {
            return BadRequest();
        }

        await _mediator.Send(command);

        return Ok();
    }
}
