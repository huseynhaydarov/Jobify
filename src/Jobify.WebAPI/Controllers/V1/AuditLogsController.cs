using Jobify.Application.Common.Models.Pagination;
using Jobify.Application.UseCases.AuditLogs.Dtos;
using Jobify.Application.UseCases.AuditLogs.Queries.GetAuditLogByJobListing;
using Jobify.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class AuditLogsController : ControllerBase
{
    private readonly ISender _mediator;

    public AuditLogsController(ISender mediator) => _mediator = mediator;

    [HttpGet("jobListings/{id}")]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<PaginatedResult<GetAuditLogByJobListingResponse>> GetAll([FromRoute] Guid id,
        [FromQuery] PagingParameters paging) =>
        await _mediator.Send(new GetAuditLogByJobListingQuery(id, paging));
}
