
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Running;
using Thing.Designer.Core.Contracts.Models;
using Thing.Designer.Core.Contracts.Services;
using Thing.Designer.Core.Tests.Benchmark;
using Thing.Designer.Tests;

// Assuming that tests are normally run with the Debug configuration.
BenchmarkRunner.Run<DbFixtureBenchmark>(
    DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
//new DbFixtureBenchmark().DbRollback();

// * Results *
//BenchmarkDotNet = v0.13.1, OS = Windows 10.0.22000
//AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
//.NET SDK=6.0.200-preview.21617.4
//  [Host]     : .NET 6.0.1(6.0.121.56705), X64 RyuJIT DEBUG
//  DefaultJob : .NET 6.0.1(6.0.121.56705), X64 RyuJIT
//
// ** All SQLite tests use In-Memory storage **
//| Method                    | Mean         |
//| ------------------------- | ------------:|
//| SqliteDbRollback          | 2.1745 ms    | 
//| SqliteDbRollbackParallel  | 12.4189 ms   |
//| ------------------------- | ------------:|
//| SqliteDbBuild             | 5.0071 ms    |
//| SqliteDbBuildParallel     | 1.6520 ms    |
//| ------------------------- | ------------:|
//| EfInMemory                | 2.1668 ms    |
//| EfInMemoryParallel        | 0.4818 ms    |

//[EtwProfiler(performExtraBenchmarksRun: true)]
public class DbFixtureBenchmark
{
    private const int OpsPerInvoke = 100;
    private readonly ParallelOptions parallelOptions = new ParallelOptions()
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount / 2,
    };

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void DbRollback()
    {
        for (int i = 0; i < OpsPerInvoke; i++)
        {
            using var fixture = new DbRollbackTestFixture();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        }
    }

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void DbRollbackParallel()
    {
        Parallel.For(0, OpsPerInvoke, parallelOptions, i =>
        {
            using var fixture = new DbRollbackTestFixture();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        });
    }

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void DbBuildSchema()
    {
        for (int i = 0; i < OpsPerInvoke; i++)
        {
            using var fixture = new BaseFixtureBenchmark();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        }
    }

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void DbBuildParallel()
    {
        Parallel.For(0, OpsPerInvoke, parallelOptions, i =>
        {
            using var fixture = new BaseFixtureBenchmark();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        });
    }

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void EfInMemory()
    {
        for (int i = 0; i < OpsPerInvoke; i++)
        {
            using var fixture = new DbInMemoryTestFixture();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        }
    }

    [Benchmark(OperationsPerInvoke = OpsPerInvoke)]
    public void EfInMemoryParallel()
    {
        Parallel.For(0, OpsPerInvoke, parallelOptions, i =>
        {
            using var fixture = new DbInMemoryTestFixture();

            SimpleDbTestAsync(fixture).GetAwaiter().GetResult();
        });
    }

    private static async Task SimpleDbTestAsync(IDbTestFixture fixture)
    {
        var sut = fixture.Services.GetService<ICustomerService>()!;

        bool doesCustomerExistBeforeCall;
        using (var db = fixture.CreateDbContext())
        {
            doesCustomerExistBeforeCall = db.Customers.Any();
        }

        // Act
        var responseFirstCall = await sut.GetCurrentCustomer(CancellationToken.None);

        // Assert
        using (var db = fixture.CreateDbContext())
        {
            var customerEntity = await db.Customers.FindAsync(responseFirstCall.Value.CustomerId);

            // Some assertions to insure the optimizer doesn't remove anything
            Assert.False(doesCustomerExistBeforeCall);
            Assert.NotNull(responseFirstCall);
            Assert.NotNull(customerEntity);
        }
    }
}

public class BaseFixtureBenchmark : BaseTestFixture
{
}
