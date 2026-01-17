using System.Net.Http.Json;

namespace Jobify.Infrastructure.Services;

public class JobSearchClient : IJobSearchClientService
{
    private readonly HttpClient _httpClient;

    public JobSearchClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<Guid>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<SearchResponse>(
            $"jobListings/search?searchTerm={searchTerm}",
            cancellationToken);

        return response?.Ids ?? [];
    }
}
