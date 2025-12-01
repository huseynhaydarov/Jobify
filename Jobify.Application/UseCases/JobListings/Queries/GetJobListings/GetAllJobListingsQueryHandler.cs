using Jobify.Application.Common.Mappings;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsQueryHandler : BaseSetting, IRequestHandler<GetAllJobListingsQuery, PaginatedList<GetAllJobListingsViewModel>>
{
    public GetAllJobListingsQueryHandler(IMapper mapper, IApplicationDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<PaginatedList<GetAllJobListingsViewModel>> Handle(GetAllJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.JobListings
            .AsNoTracking()
            .ProjectTo<GetAllJobListingsViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}
