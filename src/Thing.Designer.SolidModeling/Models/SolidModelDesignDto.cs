
using System.Runtime.Serialization;

namespace Thing.Designer.SolidModeling.Models;

[DataContract]
public class SolidModelDesignDto
{
    [DataMember(Order = 1)]
    public double Width { get; set; }

    [DataMember(Order = 2)]
    public double Height { get; set; }

    [DataMember(Order = 3)]
    public double Depth { get; set; }

    [DataMember(Order = 4)]
    public List<IFeatureDto> Features { get; set; } = new List<IFeatureDto>();
}
