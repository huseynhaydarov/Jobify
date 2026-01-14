namespace Jobify.Application.Common.Models.Pagination;

public class PagingParameters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
