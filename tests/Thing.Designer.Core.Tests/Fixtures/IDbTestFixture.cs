using Thing.Designer.Infrastructure.Data;

namespace Thing.Designer.Tests;

public interface IDbTestFixture : IDisposable
{
    TestServiceProvider Services { get; }
    string? TestUserId { get; }

    ApplicationDbContext CreateDbContext();
}
