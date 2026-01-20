using FluentValidation;

namespace Jobify.Application.UseCases.JobSeekers.Commands.CreateJobSeekers;

public class CreateJobSeekerCommandValidator : AbstractValidator<CreateJobSeekerCommand>
{
    public CreateJobSeekerCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("The Password field is incorrect. " +
                         "The password must be at least 8 characters long, contain only Latin letters, " +
                         "at least one uppercase letter, at least one lowercase letter, and one number.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
