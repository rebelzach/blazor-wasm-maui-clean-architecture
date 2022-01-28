using g3;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thing.ModelDesign;

namespace Thing.Domain.SolidModeling;

public class MeshGenerator
{
    public MeshGenerator()
    {
    }

    public string ExportStl(SolidModelDomainDesigner d)
    {
        using var sw = new StringWriter();
        var meshes = new List<WriteMesh> { new WriteMesh(d.GetMesh()) };
        new STLWriter().Write(sw, meshes, WriteOptions.Defaults);
        sw.Flush();
        return sw.ToString();
    }
}
