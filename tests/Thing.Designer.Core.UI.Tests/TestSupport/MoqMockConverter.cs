using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Thing.Designer.Core.UI.Tests.TestSupport;

class MoqMockConverter :
    WriteOnlyJsonConverter<Mock>
{
    public override void WriteJson(JsonWriter writer, Mock mock, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("MockedType");
        var typeName = mock.GetType().GetGenericArguments().FirstOrDefault()?.Name;
        serializer.Serialize(writer, typeName);

        writer.WritePropertyName("Invocations");
        writer.WriteStartArray();
        foreach (var invocation in mock.Invocations)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Method");
            serializer.Serialize(writer, invocation.Method.Name);

            writer.WritePropertyName("Arguments");
            writer.WriteStartObject();
            var parameters = invocation.Method.GetParameters().Zip(invocation.Arguments);
            foreach (var p in parameters)
            {
                writer.WritePropertyName($"{p.First.Name ?? "untitled"}");
                serializer.Serialize(writer, p.Second);
            }
            writer.WriteEndObject();

            writer.WritePropertyName("Returned");
            // Unwrap any tasks
            if (invocation.ReturnValue is Task task)
            {
                var resultProp = task.GetType().GetProperty("Result", BindingFlags.Public | BindingFlags.Instance);
                if (resultProp is null)
                {
                    serializer.Serialize(writer, "void");
                }
                else
                {
                    var returnVal = resultProp.GetValue(task);
                    serializer.Serialize(writer, returnVal);
                }
            }
            else
            {
                serializer.Serialize(writer, invocation.ReturnValue);
            }
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
