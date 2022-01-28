using System.Runtime.CompilerServices;
using Thing.Designer.Core.UI.Tests.TestSupport;

namespace Thing.Designer.Core.UI.Tests.Fixtures;

public abstract class ComponentTestFixture : TestContext
{
    public Task VerifyComponent(IRenderedFragment component, object? state = null, [CallerFilePath] string sourceFile = "")
    {
        return Verifier.Verify(new ComponentTestState(component, state), sourceFile: sourceFile);
    }
}
