using g3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thing.Domain.SolidModeling;
using Thing.Domain.SolidModeling.Models;

#nullable disable

namespace Thing.ModelDesign;

public class SolidModelDomainDesigner
{
    private readonly SolidModelDesign _design;
    private readonly Dictionary<IFeature, IFeatureDesigner> _featureDesigners =
        new Dictionary<IFeature, IFeatureDesigner>();

    public SolidModelDomainDesigner()
    {
        _design = new SolidModelDesign()
        {
            Width = 100,
            Height = 30,
            Depth = 70,
        };
    }

    public TDesigner AddFeature<TDesigner>()
        where TDesigner : class, IFeatureDesigner, new()
    {
        TDesigner designer = new TDesigner();
        _design.Features.Add(designer.Feature);
        _featureDesigners[designer.Feature] = designer;

        return designer;
    }

    public TDesigner GetFeature<TDesigner>(IFeature feature)
        where TDesigner : class, IFeatureDesigner
    {
        return null;
    }

    public void RequestBodyDimensions(double width, double height, double depth)
    {
        _design.Width = width;
        _design.Height = height;
        _design.Depth = depth;
    }

    public DMesh3 GetMesh()
    {
        var boxgen = new TrivialBox3Generator()
        {
            Clockwise = false,
            Box = new Box3d(
                new Vector3d(_design.Width / 2, _design.Depth / 2, _design.Height / 2),
                new Vector3d(_design.Width / 2, _design.Depth / 2, _design.Height / 2)),
        };
        boxgen.Generate();
        var m = boxgen.MakeDMesh();
        MeshNormals.QuickCompute(m);

        foreach (IFeatureDesigner designer in _featureDesigners.Values)
        {
            m = designer.ModifyBodyMesh(_design, m);
        }

        return m;
    }
}
