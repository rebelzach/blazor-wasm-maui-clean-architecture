
namespace Thing.Designer.Core.Tests.Services.CustomerDesignServiceTests;

[UsesVerify]
public class AddOrUpdateDesign : BaseTestFixture
{
    [Fact]
    public async Task AddWithGoodData()
    {
        var sut = Services.GetService<ICustomerDesignService>()!;

        // Act
        var designId = Guid.NewGuid();
        var response = await sut.AddOrUpdateDesignAsync(new AddOrUpdateCustomerDesignRequest(designId), CancellationToken.None);

        // Verify
        using var db2 = CreateDbContext();
        var designEntity = db2.CustomerDesigns.Find(response.Value!.Id);
        var customerEntity = db2.Customers.FirstOrDefault(c => c.ApplicationUserId == TestUserId);

        await Verifier.Verify(new { designId, response, customerId = customerEntity?.Id, designEntity });
    }
}
