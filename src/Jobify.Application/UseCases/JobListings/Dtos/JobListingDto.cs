namespace Jobify.Application.UseCases.JobListings.Dtos;

public record JobListingDto(Guid Id, JobStatus Status, DateTimeOffset PublishedAt);
