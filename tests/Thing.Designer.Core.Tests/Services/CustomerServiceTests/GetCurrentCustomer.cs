using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Thing.Designer.Core.Contracts.Models;
using Thing.Designer.Core.Contracts.Services;

namespace Thing.Designer.Tests.Services.CustomerServiceTests;

[UsesVerify]
public class GetCurrentCustomer : BaseTestFixture
{
    [Fact]
    public async Task WhenNoUserAvailable()
    {
        TestUserId = null;

        var sut = Services.GetService<ICustomerService>()!;

        // Act
        try
        {
            await sut.GetCurrentCustomer(CancellationToken.None);
        }
        catch (Exception expectedException)
        {
            // Verify
            using var db = CreateDbContext();
            int customerCount = db.Customers.Count();

            await Verifier.Verify(
                new
                {
                    customerCount,
                    expectedException
                }).IgnoreStackTrack();
        }
    }

    [Fact]
    public async Task WhenCalledOnce()
    {
        var sut = Services.GetService<ICustomerService>()!;

        bool doesCustomerExistBeforeCall;
        using (var db = CreateDbContext())
        {
            doesCustomerExistBeforeCall = db.Customers.Any();
        }

        // Act
        var responseFirstCall = await sut.GetCurrentCustomer(CancellationToken.None);

        // Assert
        using (var db = CreateDbContext())
        {
            var customerEntity = await db.Customers.FindAsync(responseFirstCall.Value.CustomerId);

            await Verifier.Verify(
                new
                {
                    ApplicationUserId = TestUserId,
                    doesCustomerExistBeforeCall,
                    responseFirstCall,
                    customerEntity,
                });
        }
    }

    [Fact]
    public async Task WhenCalledTwice()
    {
        var sut = Services.GetService<ICustomerService>()!;

        // Act
        var responseFirstCall = await sut.GetCurrentCustomer(CancellationToken.None);
        var responseSecondCall = await sut.GetCurrentCustomer(CancellationToken.None);

        // Assert
        using (var db = CreateDbContext())
        {
            var customerEntity = await db.Customers.FindAsync(responseSecondCall.Value.CustomerId);
            await Verifier.Verify(new
            {
                UserId = TestUserId,
                customerEntity,
                responseFirstCall,
                responseSecondCall,
            });
        }
    }
}
