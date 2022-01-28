using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Thing.Designer.App.WebClient;
using Thing.Designer.App.WebClient.Services;
using Thing.Designer.Core.UI.Contracts;
using Thing.Designer.Core.UI.Models;
using Thing.Designer.Infrastructure.WebClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ISignOutSessionStateManager, SignOutSessionStateManagerAdaptor>();

var backendOrigin = builder.Configuration["BackendOrigin"]!;

builder.Services
    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ThingBuilder.Blazor.Host"))
    .AddHttpClient("ThingBuilder.Blazor.Host", client => client.BaseAddress = new Uri(backendOrigin))
    .AddHttpMessageHandler(services => 
        {
            return services
                .GetRequiredService<AuthorizationMessageHandler>()
                .ConfigureHandler(authorizedUrls: new[] { backendOrigin });
        });

builder.Services.AddOptions<UserManagementPaths>()
    .Configure(opt => builder.Configuration.Bind(nameof(UserManagementPaths), opt));


builder.Services.AddApiAuthorization(opt =>
{
    opt.ProviderOptions.ConfigurationEndpoint = $"{backendOrigin}/_configuration/ThingBuilder.Blazor.Client";
    opt.AuthenticationPaths.RemoteProfilePath = $"{backendOrigin}/Identity/Account/Manage";
    opt.AuthenticationPaths.RemoteRegisterPath = $"{backendOrigin}/Identity/Account/Register";
});

builder.Services.AddThingDesignerApiClient((services, options) =>
{
    var authEnabledHandler = services.GetRequiredService<AuthorizationMessageHandler>();
    authEnabledHandler.ConfigureHandler(authorizedUrls: new[] { backendOrigin });
    authEnabledHandler.InnerHandler = new HttpClientHandler();

    options.BaseUri = backendOrigin;
    options.MessageHandler = authEnabledHandler;
});

await builder.Build().RunAsync();
