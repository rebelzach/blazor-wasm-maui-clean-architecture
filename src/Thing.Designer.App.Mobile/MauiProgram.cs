using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Thing.Designer.App.Mobile.Services;
using Thing.Designer.Core.Contracts.Services;
using Thing.Designer.Core.UI.Models;

namespace Thing.Designer.App.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .Host
            .ConfigureAppConfiguration((app, config) =>
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                config.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
            });

        builder.Services.AddScoped<ISignOutSessionStateManager, SignOutSessionStateManagerAdaptor>();
        builder.Services.AddScoped<AuthenticationStateProvider, MauiAuthenticationStateProvider>();
        builder.Services.AddScoped<ICustomerDesignService, FakeCustomerDesignService>();

        builder.Services.AddOptions<UserManagementPaths>()
            .Configure(opt => builder.Configuration.Bind(nameof(UserManagementPaths), opt));

        builder.Services.AddBlazorWebView();

        return builder.Build();
    }
}
