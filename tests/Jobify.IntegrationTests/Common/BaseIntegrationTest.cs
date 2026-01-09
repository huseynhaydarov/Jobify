namespace Jobify.IntegrationTests.Common;

public abstract class BaseIntegrationTest
{
    private protected CustomWebApplicationFactory<IApiMarker> _factory;
    protected HttpClient Client;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory<IApiMarker>();
        await _factory.InitializeAsync();
        Client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        Client.Dispose();
        await _factory.DisposeAsync();
    }
}
