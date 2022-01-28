using Ardalis.Result;
using Thing.Designer.SolidModeling.Models;
using Thing.Domain.SolidModeling;
using Thing.ModelDesign;

namespace Thing.Designer.SolidModeling;

public class SolidModelDesigner
{
    public event EventHandler<SolidModelMeshDto>? ModelMeshUpdated;

    public IResult RecalculateSolidModel(SolidModelDesignDto solidModelDesign)
    {
        var designer = new SolidModelDomainDesigner();
        designer.RequestBodyDimensions(
            solidModelDesign.Width,
            solidModelDesign.Height,
            solidModelDesign.Depth);
        foreach (var featureDto in solidModelDesign.Features)
        {
            var holeDesigner = designer.AddFeature<HoleDesigner>();
            // TODO: Make gooder holes
        }

        var asciiStl = new MeshGenerator().ExportStl(designer);

        ModelMeshUpdated?.Invoke(this, new SolidModelMeshDto { AsciiStlMeshData = asciiStl });

        return Result<bool>.Success(true);
    }
}

