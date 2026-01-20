using System.Linq;
using Jobify.Application.UseCases.JobListings.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.JobListings.Queries.GetJobListingsOdata;

public record GetAllJobListingsOdataQuery
    : IRequest<IQueryable<JobListingOdataDto>>;
