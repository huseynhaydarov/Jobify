using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Exceptions;
using Jobify.Application.Common.Extensions;
using Jobify.Application.Common.Interfaces.Data;
using Jobify.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.Employers.Commands.PositionUpdate;

public class PositionUpdateCommandHandler : BaseSetting, IRequestHandler<PositionUpdateCommand, Unit>
{
    public PositionUpdateCommandHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Unit> Handle(PositionUpdateCommand request, CancellationToken cancellationToken)
    {
        var employer = await _dbContext.Employers
                           .Where(e => e.Id == request.employerId)
                           .FirstOrDefaultAsync(cancellationToken)
                       ?? throw new NotFoundException("Employer not found");

        if (!Enum.TryParse(request.position.ToString(), out EmployerPosition enumValue))
        {
            throw new ArgumentException("Invalid employer position");
        }

        employer.Position = enumValue;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
