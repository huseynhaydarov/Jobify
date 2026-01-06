namespace Jobify.Application.UseCases.JobListings.Dtos;

public record JobListingOdataDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string? Requirements { get; init; }
    public string? Location { get; init; }

    public decimal? Salary { get; init; }
    public string? Currency { get; init; }

    public JobStatus Status { get; init; }

    public DateTimeOffset PostedAt { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }

    public int Views { get; init; }

    public CompanyOdataDto Company { get; init; } = default!;
    public EmployerOdataDto Employer { get; init; } = default!;
}
