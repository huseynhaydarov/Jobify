using System;
using MediatR;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingDetail;

public record GetJobListingDetailQuery(Guid Id) : IRequest<JobListingDetailResponse>;
