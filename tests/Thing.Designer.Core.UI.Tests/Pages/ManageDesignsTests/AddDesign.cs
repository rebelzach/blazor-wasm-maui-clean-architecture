
namespace Thing.Designer.Core.UI.Tests.Pages.ManageDesignsTests;

[UsesVerify]
public class AddDesign : ComponentTestFixture
{
    [Fact]
    public Task WhenAddClicked()
    {
        Services.AddSingleton(sp =>
        {
            var createResult = Result<AddOrUpdateCustomerDesignResponse>.Success(new AddOrUpdateCustomerDesignResponse(Guid.NewGuid())).AsContractResult();
            var listResult = Result<ListCustomerDesignsResponse>.Success(new ListCustomerDesignsResponse(new List<CustomerDesignDto>())).AsContractResult();

            return Mock.Of<ICustomerDesignService>(ds =>
                ds.AddOrUpdateDesignAsync(It.IsAny<AddOrUpdateCustomerDesignRequest>(), default) == Task.FromResult(createResult) &&
                ds.ListDesignsAsync(It.IsAny<ListCustomerDesignsRequest>(), default) == Task.FromResult(listResult));
        });

        var c = RenderComponent<ManageDesigns>();

        // Act
        c.Find("#AddDesignButton").Click();

        // Verify
        return VerifyComponent(c, new { navigationHistory = Services.GetRequiredService<FakeNavigationManager>().History });
    }
}
