namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public record GetAllJobApplicationsQuery(Guid? JobListingId, PagingParameters Parameters) :
    IRequest<PaginatedList<GetAllJobApplicationsResponse>>;
