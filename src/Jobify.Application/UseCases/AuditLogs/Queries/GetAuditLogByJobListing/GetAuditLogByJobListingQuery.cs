using Jobify.Application.UseCases.AuditLogs.Dtos;

namespace Jobify.Application.UseCases.AuditLogs.Queries.GetAuditLogByJobListing;

public record GetAuditLogByJobListingQuery(Guid Id, PagingParameters Parameters) :
    IRequest<PaginatedResult<GetAuditLogByJobListingResponse>>;
