namespace Application.UnitTests.UseCases.Companies.Commands;

[TestFixture]
public class UpdateCompanyCommandHandlerTests
{
    private IFixture _fixture;
    private DbContextOptions<ApplicationDbContext> _dbOptions;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture()
            .Customize(new AutoNSubstituteCustomization());

        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("JobifyDb")
            .Options;
    }

    [Test]
    public async Task Handle_Should_Update_Company_And_Save()
    {
        // Arrange
        var testUser = new TestAuthenticatedUser();

        await using (var context = new ApplicationDbContext(_dbOptions, testUser))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        await using var dbContext = new ApplicationDbContext(_dbOptions, testUser);

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Description = "Old Description",
            WebsiteUrl = "https://old.com",
            Industry = "Old Industry",
            CreatedById = testUser.Id
        };

        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();

        var command = new UpdateCompanyCommand(
            company.Id,
            "New Name",
            "New Description",
            "https://new.com",
            "New Industry"
        );

        var handler = new UpdateCompanyCommandHandler(dbContext, testUser);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(Unit.Value);

        var updatedCompany = await dbContext.Companies
            .FirstOrDefaultAsync(x => x.Id == company.Id);

        updatedCompany?.Name.ShouldBe(command.Name);
        updatedCompany?.Description.ShouldBe(command.Description);
        updatedCompany?.WebsiteUrl.ShouldBe(command.WebsiteUrl);
        updatedCompany?.Industry.ShouldBe(command.Industry);
    }
}
