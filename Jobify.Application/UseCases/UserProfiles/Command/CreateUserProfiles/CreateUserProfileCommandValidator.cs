using Jobify.Application.UseCases.JobApplications.Commands.CancelJobApplication;

namespace Jobify.Application.UseCases.UserProfiles.Command.CreateUserProfiles;

public class CreateUserProfileCommandValidator : AbstractValidator<CancelJobApplicationCommand>
{
    public CreateUserProfileCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("Application ID is required");
    }
}
