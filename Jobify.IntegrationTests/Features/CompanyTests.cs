namespace Jobify.IntegrationTests.Features;

[TestFixture]
public class CompanyTests : BaseIntegrationTest
{
    [Test]
    public async Task CreateCompany_ShouldReturnCreatedCompany_WhenValidRequest()
    {
        // Arrange
        var provider = _factory.Services.GetRequiredService<IAuhtenticatedTestUser>();
        provider.Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        provider.Roles = new List<string>
        {
            UserRoles.Administrator
        };

        var createCommand = new CreateCompanyCommand(
            Name: "Test Company",
            Description: "Test Description",
            WebsiteUrl: "https://api-test.com",
            Industry: "IT"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<CompanyDto>();
        matchResponse.ShouldNotBeNull();
        matchResponse.Id.ShouldNotBe(Guid.Empty);
    }
}
