namespace Jobify.Application.Common.Interfaces.Services;

public interface IJobSearchClientService
{
    Task<IReadOnlyList<Guid>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken);
}
