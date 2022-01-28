using System.Runtime.CompilerServices;
using Thing.Designer.Core.UI.Tests.TestSupport;
using VerifyTests.AngleSharp;

namespace Thing.Designer.App.UI.Tests;

public static class Startup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyBunit.Initialize();
        VerifierSettings.ScrubEmptyLines();
        VerifierSettings.ScrubLinesWithReplace(s => s.Replace("<!--!-->", ""));
        HtmlPrettyPrint.All();
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.AddExtraSettings(serializerSettings =>
            {
                var converters = serializerSettings.Converters;
                converters.Add(new ComponentTestStateConverter());
            });
        });
        VerifierSettings.ScrubLinesContaining("<script src=\"_framework/dotnet.");

        VerifyMoq.Initialize();
    }
}
