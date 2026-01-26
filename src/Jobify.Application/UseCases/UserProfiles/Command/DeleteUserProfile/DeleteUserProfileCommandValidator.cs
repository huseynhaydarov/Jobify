using FluentValidation;

namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfile;

public class DeleteUserProfileCommandValidator : AbstractValidator<DeleteUserProfileCommand>
{
    public DeleteUserProfileCommandValidator() =>
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Profile ID is required");
}
