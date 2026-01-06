namespace Jobify.Application.UseCases.JobListings.Dtos;

public record UpdateJobListingResponse(Guid Id, JobStatus Status, DateTimeOffset? UpdatedAt);
