using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<T>> PaginatedListAsync<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync(cancellationToken);

        var hasNext = items.Count > pageSize;

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
