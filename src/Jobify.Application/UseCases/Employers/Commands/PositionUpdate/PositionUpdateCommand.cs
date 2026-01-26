using System;
using MediatR;

namespace Jobify.Application.UseCases.Employers.Commands.PositionUpdate;

public record PositionUpdateCommand(Guid employerId, int position) : IRequest<Unit>;
