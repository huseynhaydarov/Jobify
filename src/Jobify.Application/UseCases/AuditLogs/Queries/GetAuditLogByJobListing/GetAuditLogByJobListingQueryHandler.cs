using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Models.Pagination;
using Jobify.Application.UseCases.AuditLogs.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.AuditLogs.Queries.GetAuditLogByJobListing;

public class GetAuditLogByJobListingQueryHandler : BaseSetting, IRequestHandler<GetAuditLogByJobListingQuery,
    PaginatedResult<GetAuditLogByJobListingResponse>>
{
    public GetAuditLogByJobListingQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedResult<GetAuditLogByJobListingResponse>> Handle(GetAuditLogByJobListingQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityId == request.Id)
            .OrderByDescending(c => c.ChangedAt)
            .Select(j => new GetAuditLogByJobListingResponse
            {
                Id = j.Id,
                EntityType = j.EntityType,
                Action = j.Action.ToString(),
                ChangedBy = j.ChangedBy,
                ChangedByType = j.ChangedByType,
                ChangedAt = j.ChangedAt,
                AuditLogDetails = j.AuditLogDetails,
                EntityId = j.EntityId
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
