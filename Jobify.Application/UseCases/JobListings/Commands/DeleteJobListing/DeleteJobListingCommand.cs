namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public record DeleteJobListingCommand(Guid Id) : IRequest<Unit>;

