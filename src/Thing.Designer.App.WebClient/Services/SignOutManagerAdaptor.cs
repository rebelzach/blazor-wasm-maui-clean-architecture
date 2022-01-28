using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Thing.Designer.Core.UI.Contracts;

namespace Thing.Designer.App.WebClient.Services;

public class SignOutSessionStateManagerAdaptor : ISignOutSessionStateManager
{
    private readonly SignOutSessionStateManager manager;

    public SignOutSessionStateManagerAdaptor(SignOutSessionStateManager manager)
    {
        this.manager = manager;
    }

    public ValueTask SetSignOutState()
    {
        return manager.SetSignOutState();
    }
}
