using Jobify.Domain.Constants;
using Jobify.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jobify.Infrastructure.Persistence.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                _logger.LogInformation("Applying database migrations...");
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { Roles.Administrator, Roles.Administrator, Roles.JobSeeker, Roles.Employer };

        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole
                {
                    IsActive = true
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        var adminEmail = "admin@localhost";
        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var administrator = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(administrator, "admin1!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(administrator, Roles.Administrator);
            }
        }
    }
}
