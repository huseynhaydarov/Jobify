namespace Jobify.Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<T>> PaginatedListAsync<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        List<T> items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync(cancellationToken);

        bool hasNext = items.Count > pageSize;

        if (hasNext)
        {
            items.RemoveAt(pageSize);
        }

        return new PaginatedResult<T>(
            items,
            pageNumber,
            pageSize,
            hasNext
        );
    }
}
