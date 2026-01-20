namespace Jobify.Application.Common.Models.Pagination;

public class PaginatedResult<T>
{
    public PaginatedResult(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        bool hasNext)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        HasNext = hasNext;
    }

    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public bool HasPrevious => PageNumber > 1;
    public bool HasNext { get; }
}
