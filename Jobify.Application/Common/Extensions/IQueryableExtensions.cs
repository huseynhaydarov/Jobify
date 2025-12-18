namespace Jobify.Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<T>> PaginatedListAsync<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);


        var items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}
