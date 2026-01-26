using System;
using Jobify.Domain.Enums;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public string? Currency { get; set; }
    public required JobStatus Status { get; set; } = JobStatus.Open;
    public required DateTimeOffset PostedAt { get; set; }
}
