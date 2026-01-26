using FluentValidation;

namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfile;

public class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
{
    public CreateUserProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("First Name is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Last Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,14}$")
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Location)
            .MaximumLength(200);

        RuleFor(x => x.Bio)
            .MaximumLength(1000);

        RuleFor(x => x.Education)
            .MaximumLength(500);

        RuleFor(x => x.Experience)
            .MaximumLength(500);
    }
}
