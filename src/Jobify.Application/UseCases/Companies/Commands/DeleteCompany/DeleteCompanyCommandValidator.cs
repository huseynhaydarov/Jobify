using FluentValidation;

namespace Jobify.Application.UseCases.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator() =>
        RuleFor(c => c.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required");
}
