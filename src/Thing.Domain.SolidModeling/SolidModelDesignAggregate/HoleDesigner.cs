using g3;

using gs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thing.Domain.SolidModeling.Models;

namespace Thing.Domain.SolidModeling;

public class HoleDesigner : IFeatureDesigner
{
    private HoleFeature _feature;

    public HoleDesigner()
    {
        _feature = new HoleFeature();
    }

    public IFeature Feature
    {
        get => _feature;
        internal set => _feature = (HoleFeature)value;
    }

    public DMesh3 ModifyBodyMesh(SolidModelDesign body, DMesh3 mesh)
    {
        double coplanarAvoidanceOffset = 0.001;
        var cylinder = new CappedCylinderGenerator()
        {
            Clockwise = true,
            BaseRadius = (float)(_feature.Diameter / 2),
            TopRadius = (float)(_feature.Diameter / 2),
            Height = (float)(_feature.Depth + coplanarAvoidanceOffset),
            Slices = 64
        };
        var cylinderMesh = cylinder.Generate().MakeDMesh();
        MeshTransforms.Rotate(cylinderMesh, new Vector3d(), Quaterniond.AxisAngleD(Vector3d.AxisX, -90));
        MeshTransforms.Translate(
            cylinderMesh,
            _feature.CenterFromLeft,
            _feature.CenterFromFront,
            body.Height + coplanarAvoidanceOffset);
        MeshNormals.QuickCompute(cylinderMesh);

        MeshBoolean2 joinOp = new MeshBoolean2()
        {
            VertexSnapTol = 0.01,
            Target = mesh,
            Tool = cylinderMesh,
        };
        joinOp.Compute(MeshBoolean2.boolOperation.Subtraction);
        MeshNormals.QuickCompute(joinOp.Result);

        return joinOp.Result;
    }
}
