// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace Jobify.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected HttpClient Client { get; private set; }
    protected CustomWebApplicationFactory Factory { get; private set; }

    [SetUp]
    public async Task SetUp()
    {
        Factory = new CustomWebApplicationFactory();
        Client = Factory.CreateClient();

        await DatabaseReset.ResetAsync(
            IntegrationTestEnvironment.PostgresConnectionString
        );
    }

    [TearDown]
    public void TearDown()
    {
        Client?.Dispose();
        Factory?.Dispose();
    }
}
