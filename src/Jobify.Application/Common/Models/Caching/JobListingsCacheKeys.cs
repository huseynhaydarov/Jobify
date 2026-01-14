namespace Jobify.Application.Common.Models.Caching;

public static class JobListingsCacheKeys
{
    public static string Page(int page, int pageSize)
    {
        return $"job-listings:page:{page}:size:{pageSize}";
    }

    public const string Registry = "job-listings:keys";
}
