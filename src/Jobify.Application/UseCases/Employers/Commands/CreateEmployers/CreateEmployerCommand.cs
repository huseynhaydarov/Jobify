using Jobify.Application.UseCases.Employers.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.Employers.Commands.CreateEmployers;

public record CreateEmployerCommand(string Email, string Password) : IRequest<EmployerDto>;
