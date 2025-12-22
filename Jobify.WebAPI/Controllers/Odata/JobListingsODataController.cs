using Jobify.Application.UseCases.JobListings.Queries.GetJobListingsOdata;

namespace Jobify.API.Controllers.Odata;

using Microsoft.AspNetCore.OData.Query;

[Route("odata/JobListings")]
public class JobListingsODataController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobListingsODataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableQuery]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = await _mediator.Send(new GetAllJobListingsOdataQuery());

        return Ok(query);
    }
}
