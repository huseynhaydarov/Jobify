namespace Jobify.Application.UseCases.Employers.Commands.PositionUpdate;

public class PositionUpdateCommandValidator : AbstractValidator<PositionUpdateCommand>
{
    public PositionUpdateCommandValidator()
    {
        RuleFor(x => x.position)
            .Must(value => Enum.IsDefined(typeof(EmployerPosition), value))
            .WithMessage("Invalid employer position.");
    }
}
