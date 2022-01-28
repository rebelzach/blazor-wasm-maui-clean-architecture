
using Thing.Designer.Core.UI.Tests.Fixtures;

namespace Thing.Designer.Core.UI.Tests.Pages.ManageDesignsTests;

[UsesVerify]
public class ListDesigns : ComponentTestFixture
{
    [Fact]
    public Task WhenEmpty()
    {
        var result = Result<ListCustomerDesignsResponse>.Success(new ListCustomerDesignsResponse(new ())).AsContractResult();

        var designService = Mock.Of<ICustomerDesignService>(ds =>
            ds.ListDesignsAsync(It.IsAny<ListCustomerDesignsRequest>(), default) == Task.FromResult(result));

        Services.AddSingleton(designService);

        // Act
        var component = RenderComponent<ManageDesigns>();

        // Verify
        return VerifyComponent(component, new { designServiceMock = Mock.Get(designService) });
    }

    [Fact]
    public Task WhenData()
    {
        // Test 9 designs in the list
        var designs = Enumerable.Range(1, 9)
            .Select(i => new CustomerDesignDto()
            {
                DesignId = Guid.NewGuid(),
                DesignName = $"Design {i}"
            }).ToList();
        var result = Result<ListCustomerDesignsResponse>.Success(new ListCustomerDesignsResponse(designs)).AsContractResult();

        var designService = Mock.Of<ICustomerDesignService>(ds =>
            ds.ListDesignsAsync(It.IsAny<ListCustomerDesignsRequest>(), default) == Task.FromResult(result));

        Services.AddSingleton(designService);

        // Act
        var component = RenderComponent<ManageDesigns>();

        // Verify
        return VerifyComponent(component, new { designServiceMock = Mock.Get(designService) });
    }
}
