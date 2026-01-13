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
