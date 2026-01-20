using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jobify.Application.Common.Interfaces.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Application.UseCases.UserProfiles.Command.DeleteUserProfiles;

public class DeleteUserProfilesCommandhHandler : IRequestHandler<DeleteUserProfilesCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteUserProfilesCommandhHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteUserProfilesCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids.Count == 0)
        {
            return Unit.Value;
        }

        var profiles = await _dbContext.UserProfiles
            .Where(p => request.Ids.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (var profile in profiles)
        {
            profile.IsDeleted = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
