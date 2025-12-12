namespace Jobify.Application.UseCases.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    DeleteCompanyCommandValidator()
    {
        RuleFor(c => c.CompanyId)
            .NotEmpty()
            .WithMessage("CompanyId is required");
    }
}
