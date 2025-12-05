namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public record GetJobListingDetailQuery(Guid Id) : IRequest<JobListingDetailViewModel>;
