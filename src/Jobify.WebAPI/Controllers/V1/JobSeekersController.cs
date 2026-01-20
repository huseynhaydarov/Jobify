using Jobify.Application.Common.Models.Pagination;
using Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;
using Jobify.Application.UseCases.JobSeekers.Queries.GetJobSeekers;
using Jobify.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var data = await _mediator.Send(command);

        return Ok(data);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<IActionResult> GetAll([FromQuery] PagingParameters parameters)
    {
        GetAllJobSeekersQuery query = new(parameters);

        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
