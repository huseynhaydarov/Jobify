namespace Jobify.Application.UseCases.JobApplications.Commands.UpdateJobApplicationStatus;

public class ApplicationStatusUpdateCommandValidator
    : AbstractValidator<ApplicationStatusUpdateCommand>
{
    public ApplicationStatusUpdateCommandValidator()
    {
        RuleFor(x => x.applicationId)
            .NotEmpty();

        RuleFor(x => x.status)
            .Must(value => Enum.IsDefined(typeof(ApplicationStatus), value))
            .WithMessage("Invalid application status.");
    }
}
