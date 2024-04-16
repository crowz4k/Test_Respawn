using Respawn;
using Test_Respawn.API;

namespace Test_Respawn.IntegrationTests;

public class CustomerAPIFactory : WebApplicationFactory<IAPIMarker>, IAsyncLifetime
{
    private Respawner _respawner = default!;
    
    private const string _connectionString = "Server=localhost;Database=Test_Respawn;User Id=sa;Password=roottoor!1;TrustServerCertificate=true;";

    
    
    public HttpClient HttpClient { get; private set; } = default!;

    public async Task ResetDatabaseAsync() => await _respawner.ResetAsync(_connectionString);

    public async Task InitializeAsync()
    {
        HttpClient = CreateClient();
        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            WithReseed = true,
            SchemasToInclude = ["dbo"],
        });
    }
    
    public new async Task DisposeAsync()
    {
        await ResetDatabaseAsync();
    }
}