using Jobify.Application.Common.Models.Pagination;
using Jobify.Application.UseCases.Companies.Commands.CreateCompanies;
using Jobify.Application.UseCases.Companies.Commands.DeleteCompany;
using Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;
using Jobify.Application.UseCases.Companies.Dtos;
using Jobify.Application.UseCases.Companies.Queries.GetCompanies;
using Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;
using Jobify.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _mediator;

    public CompaniesController(ISender mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<CreateCompanyResponse> Create([FromBody] CreateCompanyCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut]
    [Authorize(Roles = UserRoles.Administrator)]
    public async Task<UpdateCompanyResponse> Update([FromRoute] Guid id, [FromBody] UpdateCompanyRequest request)
    {
        var command = new UpdateCompanyCommand(
            id,
            request.Name,
            request.Description,
            request.WebsiteUrl,
            request.Industry);

        return await _mediator.Send(command);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<GetCompanyDetailResponse> GetDetail([FromRoute] Guid id)
    {
        var data = await _mediator.Send(new GetCompanyDetailQuery(id));

        return data;
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanViewAll)]
    public async Task<PaginatedResult<GetAllCompaniesResponse>> GetAll([FromQuery] PagingParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery(parameters);

        return await _mediator.Send(query, cancellationToken);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Policies.CanPurge)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteCompanyCommand(id));

        return NoContent();
    }
}
