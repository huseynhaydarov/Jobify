namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfiles;

public class DeleteUserProfilesCommandValidator : AbstractValidator<DeleteUserProfilesCommand>
{
    public DeleteUserProfilesCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty()
            .WithMessage("JobListing ID is required.");
    }
}
