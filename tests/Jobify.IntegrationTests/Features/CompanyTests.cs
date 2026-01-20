using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Jobify.Application.UseCases.Companies.Commands.CreateCompanies;
using Jobify.Application.UseCases.Companies.Commands.UpdateCompanies;
using Jobify.Application.UseCases.Companies.Dtos;
using Jobify.Domain.Constants;
using Shouldly;

namespace Jobify.IntegrationTests.Features;

[TestFixture]
public class CompanyTests : IntegrationTestBase
{
    [Test]
    public async Task CreateCompany_ShouldReturnCreatedCompany_WhenValidRequest()
    {
        // Arrange
        var token = await GetJwtTokenByRoleAsync(UserRoles.Administrator);

        var createCommand = new CreateCompanyCommand(
            "Test Company",
            "Test Description",
            "https://api-test.com",
            "IT"
        );

        // Act
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<CompanyDto>();
        matchResponse.ShouldNotBeNull();
        matchResponse.Id.ShouldNotBe(Guid.Empty);
    }

    [Test]
    public async Task CreateCompany_UnAuthorizedUser_Returns401Test()
    {
        // Arrange
        var createCommand = new CreateCompanyCommand(
            "Test Company",
            "Test Description",
            "https://api-test.com",
            "IT"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task UpdateCompany_UnAuthorizedUser_Returns401Test()
    {
        // Arrange
        var createCommand = Fixture.Create<UpdateCompanyCommand>();

        // Act
        var response = await Client.PostAsJsonAsync("/api/companies", createCommand);

        // Assert
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
