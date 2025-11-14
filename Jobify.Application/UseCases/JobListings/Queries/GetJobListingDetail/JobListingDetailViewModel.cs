namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public class JobListingDetailViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public string? Currency { get; set; }
    public JobStatus Status { get; set; }
    public DateTime PostedAt { get; set; }

    public string CompanyName { get; set; }
    public string EmployerEmail { get; set; }
}
