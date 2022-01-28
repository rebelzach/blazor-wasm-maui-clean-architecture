using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;

namespace Thing.Designer.Core.UI.Tests.TestSupport;

public class ComponentTestStateConverter :
    WriteOnlyJsonConverter<ComponentTestState>
{
    public override void WriteJson(JsonWriter writer, ComponentTestState componentState, JsonSerializer serializer, IReadOnlyDictionary<string, object> context)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Markup");
        var markup = PrettyPrintHtml(componentState.Component.Markup);
        serializer.Serialize(writer, markup);

        writer.WritePropertyName("ComponentInfo");
        serializer.Serialize(writer, GetComponentInstance(componentState.Component, markup));

        if (componentState.State is not null)
        {
            writer.WritePropertyName("TestState");
            serializer.Serialize(writer, componentState.State);
        }

        writer.WriteEndObject();
    }

    // From https://github.com/VerifyTests/Verify.Blazor/blob/main/src/Verify.Bunit/ComponentReader.cs
    ComponentInfo? GetComponentInstance(IRenderedFragment fragment, string markup)
    {
        var type = fragment.GetType();
        if (!type.IsGenericType)
        {
            return null;
        }

        var renderComponentInterface = type
            .GetInterfaces()
            .SingleOrDefault(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IRenderedComponentBase<>));
        if (renderComponentInterface == null)
        {
            return null;
        }

        var instanceProperty = renderComponentInterface.GetProperty("Instance")!;
        var instance = (IComponent)instanceProperty.GetValue(fragment)!;

        var all = fragment.FindAll("*");
        ComponentInfo info = new(
            instance,
            fragment.RenderCount,
            all.Count,
            markup.Length.ToString("N0"));

        return info;
    }

    // From: https://github.com/VerifyTests/Verify.AngleSharp/blob/main/src/Verify.AngleSharp/HtmlPrettyPrint.cs
    static string PrettyPrintHtml(string source)
    {
        var document = Parse(source);

        var builder = new StringBuilder();
        var formatter = new PrettyMarkupFormatter
        {
            Indentation = "  ",
            NewLine = "\n"
        };
        using var writer = new StringWriter(builder);
        document.ToHtml(writer, formatter);
        writer.Flush();
        return builder.ToString();
    }

    static INodeList Parse(string source)
    {
        var parser = new HtmlParser();
        if (source.StartsWith("<!DOCTYPE html>", StringComparison.InvariantCultureIgnoreCase) ||
            source.StartsWith("<html>", StringComparison.InvariantCultureIgnoreCase))
        {
            return parser.ParseDocument(source).ChildNodes;
        }

        var dom = parser.ParseDocument("<html><body></body></html>");
        return parser.ParseFragment(source, dom.Body!);
    }
}
