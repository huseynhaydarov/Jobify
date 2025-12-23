namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsQueryHandler : BaseSetting,
    IRequestHandler<GetAllJobListingsQuery, PaginatedResult<GetAllJobListingsResponse>>
{
    public GetAllJobListingsQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedResult<GetAllJobListingsResponse>> Handle(GetAllJobListingsQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = _dbContext.JobListings
            .AsNoTracking()
            .Select(c => new GetAllJobListingsResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Requirements = c.Requirements,
                Location = c.Location,
                Salary = c.Salary,
                Currency =  c.Currency,
                Status =  c.Status,
                PostedAt =  c.PostedAt,
                Views =  c.Views,
            });

        return await queryable.PaginatedListAsync(
            request.Parameters.PageNumber,
            request.Parameters.PageSize,
            cancellationToken);
    }
}
