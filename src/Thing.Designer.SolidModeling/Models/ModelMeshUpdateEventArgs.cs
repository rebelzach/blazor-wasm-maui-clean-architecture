namespace Thing.Designer.SolidModeling.Models;

public class ModelMeshUpdateEventArgs
{
    public ModelMeshUpdateEventArgs(SolidModelMeshDto mesh)
    {
        Mesh = mesh;
    }

    public SolidModelMeshDto Mesh { get; }
}

