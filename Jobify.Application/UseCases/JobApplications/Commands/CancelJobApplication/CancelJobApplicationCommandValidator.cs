namespace Jobify.Application.UseCases.JobApplications.Commands.CancelJobApplication;

public class CancelJobApplicationCommandValidator : AbstractValidator<CancelJobApplicationCommand>
{
    public CancelJobApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("Application ID is required");
    }
}
