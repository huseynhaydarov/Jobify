using System.Net;

namespace Jobify.IntegrationTests.Features;

[TestFixture]
public class CompanyTests : IntegrationTestBase
{
    [Test]
    public async Task CreateCompany_UnAuthorizedUser_Returns401Test()
    {
        // Arrange
        var createCommand = new CreateCompanyCommand(
            Name: "Test Company",
            Description: "Test Description",
            WebsiteUrl: "https://api-test.com",
            Industry: "IT"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
