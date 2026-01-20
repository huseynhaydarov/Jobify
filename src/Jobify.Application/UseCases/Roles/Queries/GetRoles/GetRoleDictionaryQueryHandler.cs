using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Roles.Queries.GetRoles;

public class GetRoleDictionaryQueryHandler : BaseSetting,
    IRequestHandler<GetRoleDictionaryQuery, List<GetRoleDictionaryResponse>>
{
    public GetRoleDictionaryQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<GetRoleDictionaryResponse>> Handle(GetRoleDictionaryQuery request,
        CancellationToken cancellationToken) =>
        await _dbContext.Roles
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .Select(r => new GetRoleDictionaryResponse { Id = r.Id, Name = r.Name })
            .ToListAsync(cancellationToken);
}
