using System;
using Jobify.Application.UseCases.JobApplications.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.JobApplications.Commands.CancelJobApplication;

public record CancelJobApplicationCommand(Guid ApplicationId) : IRequest<CancelJobApplicationResponse>;
