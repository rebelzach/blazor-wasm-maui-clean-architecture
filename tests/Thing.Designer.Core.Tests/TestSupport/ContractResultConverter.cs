using System.Collections.Generic;
using Newtonsoft.Json;
using VerifyTests;

namespace Thing.Designer.Core.Tests.TestSupport;

class ContractResultConverter : WriteOnlyJsonConverter<IContractResult>
{
    public override void WriteJson(JsonWriter writer, IContractResult result, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(result.Value));
        serializer.Serialize(writer, result.Value);

        writer.WriteEndObject();
    }
}
