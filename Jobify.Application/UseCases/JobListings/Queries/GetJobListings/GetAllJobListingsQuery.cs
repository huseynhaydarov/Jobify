namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListings;

public record GetAllJobListingsQuery(PagingParameters Parameters) : IRequest<PaginatedList<GetAllJobListingsViewModel>>;
