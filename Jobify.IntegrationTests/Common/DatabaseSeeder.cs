namespace Jobify.IntegrationTests.Common;

public static class DatabaseSeeder
{
    public static async Task SeedTestUsersAsync(ApplicationDbContext db, IPasswordHasherService hasher)
    {
        if (!await db.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "admin@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("Admin123!")
            };

            var employerUser = new User
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Email = "employer@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("Employer123!")
            };

            var jobSeekerUser = new User
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Email = "jobseeker@test.com",
                IsActive = true,
                PasswordHash = await hasher.HashPasswordAsync("JobSeeker123!")
            };

            var roles = await db.Roles.ToListAsync();

            adminUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.Administrator).Id,
                UserId = adminUser.Id
            });

            employerUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.Employer).Id,
                UserId = employerUser.Id
            });

            jobSeekerUser.UserRoles.Add(new UserRole
            {
                RoleId = roles.First(r => r.Name == UserRoles.JobSeeker).Id,
                UserId = jobSeekerUser.Id
            });

            await db.Users.AddRangeAsync(adminUser, employerUser, jobSeekerUser);
            await db.SaveChangesAsync();
        }
    }
}

