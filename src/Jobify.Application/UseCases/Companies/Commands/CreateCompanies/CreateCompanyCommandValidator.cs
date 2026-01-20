using System;
using FluentValidation;

namespace Jobify.Application.UseCases.Companies.Commands.CreateCompanies;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(200)
            .WithMessage("Website URL cannot exceed 200 characters.")
            .Must(BeValidUrl);

        RuleFor(x => x.Industry)
            .MaximumLength(100).WithMessage("Industry cannot exceed 100 characters.");
    }

    private bool BeValidUrl(string? url) => Uri.TryCreate(url, UriKind.Absolute, out _);
}
