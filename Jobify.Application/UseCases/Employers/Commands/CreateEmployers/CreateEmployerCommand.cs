namespace Jobify.Application.UseCases.Employers.Commands.CreateEmployers;

public record CreateEmployerCommand(string Email, string Password) : IRequest<EmployerDto>;
