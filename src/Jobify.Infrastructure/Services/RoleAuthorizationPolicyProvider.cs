using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Jobify.Infrastructure.Services;

public class RoleAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _defaultProvider;
    private readonly IServiceScopeFactory _scopeFactory;

    public RoleAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _defaultProvider = new DefaultAuthorizationPolicyProvider(options);
        _scopeFactory = scopeFactory;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var role = await dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.IsActive &&
                r.Name.ToLower() == policyName.ToLower());

        if (role != null)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(role.Name)
                .Build();

            return policy;
        }

        return await _defaultProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _defaultProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _defaultProvider.GetFallbackPolicyAsync();
}
