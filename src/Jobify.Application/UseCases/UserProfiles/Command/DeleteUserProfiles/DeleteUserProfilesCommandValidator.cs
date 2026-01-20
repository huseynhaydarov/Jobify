using FluentValidation;

namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfiles;

public class DeleteUserProfilesCommandValidator : AbstractValidator<DeleteUserProfilesCommand>
{
    public DeleteUserProfilesCommandValidator() =>
        RuleFor(x => x.Ids)
            .NotEmpty()
            .WithMessage("Profile ID is required.");
}
