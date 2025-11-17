namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public class DeleteJobListingCommandValidator : AbstractValidator<DeleteJobListingCommand>
{
    public DeleteJobListingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("JobListing ID is required.");
    }
}
