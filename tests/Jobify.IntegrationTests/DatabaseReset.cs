using System.Threading.Tasks;
using Npgsql;
using Respawn;

namespace Jobify.IntegrationTests;

public static class DatabaseReset
{
    private static Respawner _respawner = null!;

    public static async Task ResetAsync(string connectionString)
    {
        await using NpgsqlConnection conn = new(connectionString);
        await conn.OpenAsync();
        _respawner = await Respawner.CreateAsync(conn,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" },
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
        await _respawner.ResetAsync(conn);
    }
}
