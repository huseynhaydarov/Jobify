using Jobify.Domain.Constants;

namespace Jobify.Infrastructure.Persistence.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = services.GetRequiredService<IPasswordHasherService>();

        await SeedRolesAsync(db);
        await SeedUsersAsync(db, hasher);
    }

    private static async Task SeedUsersAsync(
        ApplicationDbContext db,
        IPasswordHasherService hasher)
    {
        if (!await db.Users.AnyAsync())
        {
            User adminUser = new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "admin@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("Admin123!")
            };

            User employerUser = new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Email = "employer@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("Employer123!")
            };

            User jobSeekerUser = new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Email = "jobseeker@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("JobSeeker123!")
            };

            var roles = await db.Roles.ToListAsync();

            adminUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.Administrator).Id, UserId = adminUser.Id
            });

            employerUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.Employer).Id, UserId = employerUser.Id
            });

            jobSeekerUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.JobSeeker).Id, UserId = jobSeekerUser.Id
            });

            await db.Users.AddRangeAsync(adminUser, employerUser, jobSeekerUser);
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(ApplicationDbContext db)
    {
        if (await db.Roles.AnyAsync())
        {
            return;
        }

        var roles = new List<Role>
        {
            new()
            {
                Id = Guid.Parse("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                Name = UserRoles.Administrator,
                Description = "Administrator of the system",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                Name = UserRoles.Employer,
                Description = "Employer of the system",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.Parse("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                Name = UserRoles.JobSeeker,
                Description = "Job seeker of the system",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await db.Roles.AddRangeAsync(roles);
        await db.SaveChangesAsync();
    }
}
