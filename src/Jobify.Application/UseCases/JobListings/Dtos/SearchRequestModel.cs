using SearchService.Contracts.Requests;

namespace Jobify.Application.UseCases.JobListings.Dtos;

public record SearchRequestModel
{
    public required SearchRequest SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

