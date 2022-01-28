using System.Runtime.CompilerServices;
using DiffEngine;
using Thing.Designer.Core.Tests.TestSupport;
using VerifyTests;

namespace Thing.Designer.Core.Tests;

public static class Startup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.AddExtraSettings(serializerSettings =>
                serializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include);
            //settings.AddExtraSettings(serializerSettings => 
            //    serializerSettings.Converters.Add(new ContractResultConverter()));
        });
    }
}
