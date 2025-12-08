namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public class GetUserProfileDetailQueryHandler : BaseSetting, IRequestHandler<GetUserProfileDetailQuery, GetUserProfileDetailVievModel>
{
    public GetUserProfileDetailQueryHandler(
        IMapper mapper,
        IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<GetUserProfileDetailVievModel> Handle(GetUserProfileDetailQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
                   .Where(j => j.Id == request.Id)
                   .ProjectTo<GetUserProfileDetailVievModel>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Profile not found");
    }
}
