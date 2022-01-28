using System.Data.Common;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

public class DbInMemoryTestFixture : IDbTestFixture
{
    private string? _testUserId;

    public DbInMemoryTestFixture(Action<IServiceCollection>? configureServices = null)
    {
        Services = new TestServiceProvider();

        var dbGuid = Guid.NewGuid().ToString();

        Services.AddThingDesignerInfrastructure(options => options
            .UseInMemoryDatabase(dbGuid, mo => mo.EnableNullChecks())
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        SeedDatabaseAsync().GetAwaiter().GetResult();

        configureServices?.Invoke(Services);
    }

    public TestServiceProvider Services { get; }

    public ApplicationDbContext CreateDbContext()
    {
        var context = new ApplicationDbContext(Services.GetService<DbContextOptions>()!, Services.GetService<IOptions<OperationalStoreOptions>>()!);

        return context;
    }

    private async Task SeedDatabaseAsync()
    {
        var user = new ApplicationUser
        {
            UserName = "testing@thingbuilder.fake"
        };
        await Services.GetService<UserManager<ApplicationUser>>()!.CreateAsync(user);
        _testUserId = user.Id;
    }

    public string TestUserId => _testUserId!;

    public void Dispose()
    {
        Services.Dispose();
    }
}
