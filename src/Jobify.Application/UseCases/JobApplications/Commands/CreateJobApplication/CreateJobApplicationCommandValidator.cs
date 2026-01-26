using FluentValidation;
using Jobify.Application.Common.Interfaces.Data;

namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateJobApplicationCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.JobListingId)
            .NotEmpty()
            .WithMessage("JobListing ID is required.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(2000)
            .WithMessage("Cover letter cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CoverLetter));
    }
}
