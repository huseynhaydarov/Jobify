using System;
using Jobify.Domain.Enums;

namespace Jobify.Application.UseCases.JobApplications.Dtos;

public record JobApplicationDto(Guid Id, JobStatus Status);
