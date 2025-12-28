[TestFixture]
public class CreateCompanyCommandHandlerTests
{
    private IFixture _fixture;
    private DbContextOptions<ApplicationDbContext> _dbOptions;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture()
            .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("JobifyDb")
            .Options;
    }

    [Test]
    public async Task Handle_Should_Add_Entity_And_Save()
    {
        // Arrange
        var testUser = new TestAuthenticatedUser();

        await using (var context = new ApplicationDbContext(_dbOptions, testUser))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        await using var dbContext = new ApplicationDbContext(_dbOptions, testUser);

        var handler = new CreateCompanyCommandHandler(dbContext, testUser);

        var command = _fixture.Build<CreateCompanyCommand>()
            .Create();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);

        var company = await dbContext.Companies.SingleAsync();

        company.Id.ShouldBe(result.Id);
    }
}
