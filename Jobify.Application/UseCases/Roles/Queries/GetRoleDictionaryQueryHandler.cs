using AutoMapper;
using AutoMapper.QueryableExtensions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Roles.Queries;

public class GetRoleDictionaryQueryHandler : BaseSetting, IRequestHandler<GetRoleDictionaryQuery, List<GetRoleDictionaryViewModel>>
{
    public GetRoleDictionaryQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<List<GetRoleDictionaryViewModel>> Handle(GetRoleDictionaryQuery request, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Roles
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .AsQueryable();

        return await queryable
            .ProjectTo<GetRoleDictionaryViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
