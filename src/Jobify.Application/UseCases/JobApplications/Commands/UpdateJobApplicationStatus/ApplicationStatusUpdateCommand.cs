using System;
using MediatR;

namespace Jobify.Application.UseCases.JobApplications.Commands.UpdateJobApplicationStatus;

public record ApplicationStatusUpdateCommand(
    Guid applicationId,
    int status) : IRequest<Unit>;
