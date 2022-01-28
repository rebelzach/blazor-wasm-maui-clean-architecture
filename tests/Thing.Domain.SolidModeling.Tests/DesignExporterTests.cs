
using System.IO;
using System.Threading.Tasks;
using Thing.Domain.SolidModeling;
using VerifyXunit;
using Xunit;

namespace Thing.ModelDesign.Tests;

[UsesVerify]
public class DesignExporterTests
{
    [Fact]
    public Task ExportsStl()
    {
        var designer = new SolidModelDomainDesigner();
        designer.RequestBodyDimensions(120, 30, 30);
        designer.AddFeature<HoleDesigner>();

        var asciiStl = new MeshGenerator().ExportStl(designer);
        return Verifier
            .Verify(System.Text.Encoding.UTF8.GetBytes(asciiStl))
            .UseExtension("stl");
    }
}
