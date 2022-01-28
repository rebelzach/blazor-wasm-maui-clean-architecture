
using Microsoft.Extensions.DependencyInjection;

namespace Thing.Designer.Core.Tests.Services.CustomerDesignServiceTests;

[UsesVerify]
public class ListDesigns : BaseTestFixture
{
    [Fact]
    public async Task WithADesign()
    {
        var sut = Services.GetRequiredService<ICustomerDesignService>();

        await sut.AddOrUpdateDesignAsync(new AddOrUpdateCustomerDesignRequest(Guid.NewGuid()));
        await sut.AddOrUpdateDesignAsync(new AddOrUpdateCustomerDesignRequest(Guid.NewGuid()));

        // Act
        var response = await sut.ListDesignsAsync(new ListCustomerDesignsRequest(), CancellationToken.None);

        // Verify
        await Verifier.Verify(response);
    }
}
