namespace Jobify.Application.UseCases.JobListings.Commands.UpdateJobListing;

public class UpdateJobListingCommandValidator : AbstractValidator<UpdateJobListingCommand>
{
    public UpdateJobListingCommandValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Job listing ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Job title is required.")
            .MaximumLength(150)
            .WithMessage("Job title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Requirements)
            .MaximumLength(2000)
            .WithMessage("Requirements cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Requirements));

        RuleFor(x => x.Location)
            .MaximumLength(250)
            .WithMessage("Location cannot exceed 250 characters.")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .WithMessage("Salary must be greater than zero.")
            .When(x => x.Salary.HasValue);

        RuleFor(x => x.Currency)
            .Length(3)
            .WithMessage("Currency must be a 3-letter ISO code.")
            .When(x => !string.IsNullOrEmpty(x.Currency));

        RuleFor(x => x.ExpireDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Expire date must be in the future.")
            .When(x => x.ExpireDate.HasValue);

        RuleFor(x => x.Status)
            .NotEmpty();
    }
}
