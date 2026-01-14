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
        using IServiceScope scope = _scopeFactory.CreateScope();
        IApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        Role? role = await dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.IsActive &&
                r.Name.ToLower() == policyName.ToLower());

        if (role != null)
        {
            AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
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
