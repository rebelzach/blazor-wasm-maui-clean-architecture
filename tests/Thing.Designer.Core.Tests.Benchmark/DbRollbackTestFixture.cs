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

public class DbRollbackTestFixture : IDbTestFixture
{
    private const string _inMemoryConnectionString = "Data Source=CoreTests;Mode=Memory;Cache=Shared";
    private static DbConnection? _inMemoryPreservingConnection;
    private static string? _testUserId;
    private static readonly object _lock = new object();

    private DbTransaction _dbTransaction;

    public DbRollbackTestFixture()
    {
        Services = new TestServiceProvider();

        lock (_lock)
        {
            if (_inMemoryPreservingConnection is null)
            {
                var connection = new SqliteConnection(_inMemoryConnectionString);
                connection.Open();

                SeedDatabaseAsync(connection).GetAwaiter().GetResult();
                _inMemoryPreservingConnection = connection;
            }
        }

        var tempConnection = new SqliteConnection(_inMemoryConnectionString);
        tempConnection.Open();

        // Start a transaction that will normally rollback when disposed
        _dbTransaction = tempConnection.BeginTransaction();

        Services.AddThingDesignerInfrastructure(options => options.UseSqlite(tempConnection));

        Services.Replace(ServiceDescriptor.Scoped(sp =>
        {
            var context = new ApplicationDbContext(Services.GetService<DbContextOptions>()!, Services.GetService<IOptions<OperationalStoreOptions>>()!);
            context.Database.UseTransaction(_dbTransaction);
            return context;
        }));
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
        _dbTransaction?.Rollback();
        _dbTransaction?.Dispose();
        Services.Dispose();
    }
}
