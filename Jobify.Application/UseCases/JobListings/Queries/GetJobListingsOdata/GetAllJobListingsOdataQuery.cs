namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingsOdata;

public record GetAllJobListingsOdataQuery
    : IRequest<IQueryable<JobListingOdataDto>>;

