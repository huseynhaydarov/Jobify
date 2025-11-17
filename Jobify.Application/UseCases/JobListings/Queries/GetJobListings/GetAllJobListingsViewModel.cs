namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public class GetAllJobListingsViewModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public string? Currency { get; set; }
    public required JobStatus Status { get; set; } = JobStatus.Open;
    public required DateTime PostedAt { get; set; }
    public int Views { get; set; }
}
