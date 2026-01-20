using System;
using Jobify.Domain.Enums;

namespace Jobify.Application.UseCases.JobListings.Dtos;

public record UpdateJobListingResponse(Guid Id, JobStatus Status, DateTimeOffset? UpdatedAt);
