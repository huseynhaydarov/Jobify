namespace Application.UnitTests.UseCases.Companies.Commands;

[TestFixture]
public class DeleteCompanyCommandHandlerTests
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
    public async Task Handle_Should_Delete_Company()
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
            Name = "Jobify Inc",
            CreatedById = testUser.Id,
            IsDeleted = false
        };

        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteCompanyCommandHandler(dbContext, testUser);

        var command = new DeleteCompanyCommand(company.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBe(Unit.Value);

        var deletedCompany = await dbContext.Companies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == command.CompanyId);

        deletedCompany!.IsDeleted.ShouldBeTrue();
    }
}
