using System;

namespace Jobify.Application.UseCases.JobApplications.Dtos;

public record CancelJobApplicationResponse(Guid Id, DateTimeOffset? WithdrawnAt);
