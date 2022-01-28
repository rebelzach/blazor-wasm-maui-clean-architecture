using g3;
using Thing.Domain.SolidModeling.Models;

namespace Thing.Domain.SolidModeling;

public interface IFeatureDesigner
{
    IFeature Feature { get; }

    DMesh3 ModifyBodyMesh(SolidModelDesign body, DMesh3 mesh);
}
