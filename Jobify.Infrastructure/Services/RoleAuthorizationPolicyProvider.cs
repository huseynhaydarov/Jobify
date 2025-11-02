using Jobify.Application.Common.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jobify.Infrastructure.Services
{
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

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _defaultProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _defaultProvider.GetFallbackPolicyAsync();
        }
    }
}
