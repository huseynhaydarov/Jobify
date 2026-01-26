using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Jobify.IntegrationTests;

[SuppressMessage(
    "NUnit",
    "NUnit1032",
    Justification = "Disposed in OneTimeTearDown of SetUpFixture")]
[SetUpFixture]
public class IntegrationTestEnvironment
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder("postgres:15-alpine")
#if !DEBUG
            .WithDockerEndpoint("tcp://docker:2375")
#endif
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder("rabbitmq:3.11")
#if !DEBUG
            .WithDockerEndpoint("tcp://docker:2375")
#endif
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:7.0")
#if !DEBUG
            .WithDockerEndpoint("tcp://docker:2375")
#endif
        .Build();

    public static string PostgresConnectionString { get; private set; } = string.Empty;
    public static string RedisConnectionString { get; private set; } = string.Empty;
    public static string RabbitMqConnectionString { get; private set; } = string.Empty;

    [OneTimeSetUp]
    public async Task GlobalSetupAsync()
    {
        await Task.WhenAll(
            _postgresSqlContainer.StartAsync(),
            _redisContainer.StartAsync(),
            _rabbitMqContainer.StartAsync()
        );

        PostgresConnectionString = _postgresSqlContainer.GetConnectionString();
        RedisConnectionString = _redisContainer.GetConnectionString();
        RabbitMqConnectionString = $"{_rabbitMqContainer.Hostname}:" +
                                   $"{_rabbitMqContainer.GetMappedPublicPort(5672)}";
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown() =>
        await Task.WhenAll(
            _postgresSqlContainer.DisposeAsync().AsTask(),
            _redisContainer.DisposeAsync().AsTask(),
            _rabbitMqContainer.DisposeAsync().AsTask()
        );
}
