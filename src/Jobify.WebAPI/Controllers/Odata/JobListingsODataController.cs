using Jobify.Application.UseCases.JobListings.Dtos;

namespace Jobify.WebAPI.Controllers.Odata;

[Route("odata/JobListings")]
public class JobListingsODataController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobListingsODataController(IMediator mediator) => _mediator = mediator;

    [EnableQuery]
    [HttpGet]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<IActionResult> Get()
    {
        var query = await _mediator.Send(new GetAllJobListingsOdataQuery());

        return Ok(query);
    }
}
