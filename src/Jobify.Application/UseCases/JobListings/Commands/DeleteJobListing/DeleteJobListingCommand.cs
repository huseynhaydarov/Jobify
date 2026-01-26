using System;
using Jobify.Application.UseCases.JobListings.Dtos;
using MediatR;

namespace Jobify.Application.UseCases.JobListings.Commands.DeleteJobListing;

public record DeleteJobListingCommand(Guid Id) : IRequest<CloseJobListingResponse>;
