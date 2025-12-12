namespace Jobify.Application.UseCases.Roles.Queries.GetRoles;

public class GetRoleDictionaryQueryHandler : BaseSetting, IRequestHandler<GetRoleDictionaryQuery, List<GetRoleDictionaryViewModel>>
{
    public GetRoleDictionaryQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<List<GetRoleDictionaryViewModel>> Handle(GetRoleDictionaryQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ProjectToListAsync<GetRoleDictionaryViewModel>(_mapper.ConfigurationProvider, cancellationToken);
    }
}
