using FluentValidation;

namespace Jobify.Application.UseCases.Employers.Commands.JoinCompany;

public class JoinCompanyCommandValidator : AbstractValidator<JoinCompanyCommand>
{
    public JoinCompanyCommandValidator() =>
        RuleFor(c => c.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required");
}
