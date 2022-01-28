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
using Thing.Designer.Core.InfrastructureContracts;
using Thing.Designer.Infrastructure.Data;
using Thing.Designer.Infrastructure.Local;
using Thing.Designer.Infrastructure.Local.Models;

namespace Thing.Designer.Tests;

[UsesVerify]
public abstract class BaseTestFixture : IDisposable, IDbTestFixture
{
    private DbConnection? _connection;

    public BaseTestFixture(Action<IServiceCollection>? configureServices = null)
    {
        Services = new TestServiceProvider();

        var connString = "Filename=:memory:";
        var connection = new SqliteConnection(connString);
        connection.Open();

        SeedDatabaseAsync(connection).GetAwaiter().GetResult();
        _connection = connection;

        Services.AddThingDesignerInfrastructure(options => options.UseSqlite(_connection));

        Services.Replace(ServiceDescriptor.Transient<ICurrentApplicationUser>(sp => new CurrentApplicationUser(TestUserId)));
        configureServices?.Invoke(Services);
    }

    public TestServiceProvider Services { get; }

    public ApplicationDbContext CreateDbContext()
    {
        // We want the caller to own the disposal of this context so we don't use DI to get it.
        var context = new ApplicationDbContext(Services.GetService<DbContextOptions>()!, Services.GetService<IOptions<OperationalStoreOptions>>()!);

        return context;
    }

    private async Task SeedDatabaseAsync(SqliteConnection connection)
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
        TestUserId = user.Id;
    }

    public string? TestUserId { get; set; }

    public void Dispose()
    {
        Services.Dispose();
    }
}
