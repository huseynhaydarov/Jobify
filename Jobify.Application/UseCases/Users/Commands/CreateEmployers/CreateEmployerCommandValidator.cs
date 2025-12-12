namespace Jobify.Application.UseCases.Users.Commands.CreateEmployers;

public class CreateEmployerCommandValidator : AbstractValidator<CreateEmployerCommand>
{
    public CreateEmployerCommandValidator()
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
