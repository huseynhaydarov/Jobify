using System.Net;

namespace Jobify.IntegrationTests.Features;

[TestFixture]
public class CompanyTests : IntegrationTestBase
{
    [Test]
    public async Task CreateCompany_UnAuthorizedUser_Returns401Test()
    {
        // Arrange
        CreateCompanyCommand createCommand = new(
            "Test Company",
            "Test Description",
            "https://api-test.com",
            "IT"
        );

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
