using FluentValidation;

namespace Jobify.Application.UseCases.Auths.ChangePassword.Commands;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
            .WithMessage("The Password field is incorrect. " +
                         "The password must be at least 8 characters long, contain only Latin letters, " +
                         "at least one uppercase letter, at least one lowercase letter, and one number.");

        RuleFor(x => x)
            .Must(x => x.NewPassword == x.ConfirmNewPassword)
            .WithMessage("Passwords do not match.");
    }
}
