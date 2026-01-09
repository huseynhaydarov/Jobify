namespace Jobify.Application.UseCases.JobListings.Dtos;

public record CloseJobListingResponse(Guid Id, JobStatus Status, DateTimeOffset? ClosedAt);
