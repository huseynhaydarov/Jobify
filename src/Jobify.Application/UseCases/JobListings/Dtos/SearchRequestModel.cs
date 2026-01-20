namespace Jobify.Application.UseCases.JobListings.Dtos;

public record SearchRequestModel
{
    public required string SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

