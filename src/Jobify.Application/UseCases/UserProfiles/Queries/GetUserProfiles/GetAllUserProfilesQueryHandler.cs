using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Application.Common.Models.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public class GetAllUserProfilesQueryHandler : BaseSetting, IRequestHandler<GetAllUserProfilesQuery,
    PaginatedResult<GetAllUserProfilesResponse>>
{
    public GetAllUserProfilesQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedResult<GetAllUserProfilesResponse>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.UserProfiles
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(p => new GetAllUserProfilesResponse
            {
                Id = p.Id,
                FullName = p.FirstName + " " + p.LastName,
                PhoneNumber = p.PhoneNumber,
                Location = p.Location,
                Bio = p.Bio,
                Education = p.Education,
                Experience = p.Experience,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.ModifiedAt
            });

        return await queryable.PaginatedListAsync(
            request.PagingParameters.PageNumber,
            request.PagingParameters.PageSize,
            cancellationToken);
    }
}
