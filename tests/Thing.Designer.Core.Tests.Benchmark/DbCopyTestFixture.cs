using System.Data.Common;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Thing.Designer.Infrastructure.Data;
using Thing.Designer.Infrastructure.Local;
using Thing.Designer.Infrastructure.Local.Models;
using Thing.Designer.Tests;

namespace Thing.Designer.Core.Tests.Benchmark;

public class DbCopyTestFixture : IDbTestFixture
{
    private const string _connectionString = "Data Source=CoreTests.sqlite";
    private static bool _isDbSetup;
    private static string? _testUserId;
    private static readonly object _lock = new object();

    private string? _sqliteFileName;
    private SqliteConnection? _tempConnection;

    public DbCopyTestFixture(Action<IServiceCollection>? configureServices = null)
    {
        Services = new TestServiceProvider();

        lock (_lock)
        {
            if (!_isDbSetup)
            {
                using var connection = new SqliteConnection(_connectionString);

                if (File.Exists(connection.DataSource))
                    File.Delete(connection.DataSource);

                connection.Open();

                //var setupWal1 = connection.CreateCommand();
                //setupWal1.CommandText = "PRAGMA journal_mode=WAL;";
                //setupWal1.ExecuteNonQuery();

                SeedDatabaseAsync(connection).GetAwaiter().GetResult();

                _isDbSetup = true;

                connection.Close();
            }
        }

        _sqliteFileName = $"CoreTests_{Guid.NewGuid()}.sqlite";
        File.Copy("CoreTests.sqlite", _sqliteFileName);

        _tempConnection = new SqliteConnection($"Data Source={_sqliteFileName}");

        _tempConnection.Open();

        Services.AddThingDesignerInfrastructure(options => options.UseSqlite(_tempConnection));

        //Services.Replace(ServiceDescriptor.Scoped(sp =>
        //{
        //    var context = new ApplicationDbContext(Services.GetService<DbContextOptions>()!, Services.GetService<IOptions<OperationalStoreOptions>>()!);
        //    context.Database.UseTransaction(_dbTransaction);
        //    return context;
        //}));

        configureServices?.Invoke(Services);
    }

    public TestServiceProvider Services { get; }

    public ApplicationDbContext CreateDbContext()
    {
        var context = new ApplicationDbContext(Services.GetService<DbContextOptions>()!, Services.GetService<IOptions<OperationalStoreOptions>>()!);

        return context;
    }

    private static async Task SeedDatabaseAsync(SqliteConnection connection)
    {
        using var seedServices = new TestServiceProvider();

        seedServices.AddThingDesignerInfrastructure(options => options.UseSqlite(connection));
        var context = seedServices.GetService<ApplicationDbContext>()!;

        context.Database.EnsureCreated();
        var user = new ApplicationUser
        {
            UserName = "testing@thingbuilder.fake"
        };
        await seedServices.GetService<UserManager<ApplicationUser>>()!.CreateAsync(user);
        _testUserId = user.Id;
    }

    public string TestUserId => _testUserId!;

    public void Dispose()
    {
        _tempConnection?.Close();
        _tempConnection?.Dispose();
        Services.Dispose();
    }
}
