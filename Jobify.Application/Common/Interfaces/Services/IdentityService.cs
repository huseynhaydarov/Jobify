using Jobify.Application.Common.Models;

namespace Jobify.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
