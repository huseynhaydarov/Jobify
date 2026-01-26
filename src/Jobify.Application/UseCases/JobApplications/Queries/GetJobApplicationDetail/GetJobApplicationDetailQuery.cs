using System;
using MediatR;

namespace Jobify.Application.UseCases.JobApplications.Queries.GetJobApplicationDetail;

public record GetJobApplicationDetailQuery(Guid Id)
    : IRequest<GetJobApplicationDetailResponse>;
