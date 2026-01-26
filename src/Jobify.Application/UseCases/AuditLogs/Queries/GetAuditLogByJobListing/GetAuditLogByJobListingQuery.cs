using System;
using Jobify.Application.Common.Models.Pagination;
using Jobify.Application.UseCases.AuditLogs.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.AuditLogs.Queries.GetAuditLogByJobListing;

public record GetAuditLogByJobListingQuery(Guid Id, PagingParameters Parameters) :
    IRequest<PaginatedResult<GetAuditLogByJobListingResponse>>;
