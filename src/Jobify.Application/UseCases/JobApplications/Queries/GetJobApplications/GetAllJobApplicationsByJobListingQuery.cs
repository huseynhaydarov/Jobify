namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplications;

public record GetAllJobApplicationsByJobListingQuery(Guid? JobListingId, PagingParameters Parameters) :
    IRequest<PaginatedResult<GetAllJobApplicationsByJobListingResponse>>;
