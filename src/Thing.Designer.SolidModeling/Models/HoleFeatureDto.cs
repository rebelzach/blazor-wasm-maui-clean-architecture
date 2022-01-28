using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Thing.Designer.SolidModeling.Models;

[DataContract]
public class HoleFeatureDto : IFeatureDto
{
    [DataMember(Order = 1)]
    public double Diameter { get; set; } = 10;
    [DataMember(Order = 2)]
    public double Depth { get; set; } = 10;
    [DataMember(Order = 3)]
    public double CenterFromLeft { get; set; } = 15;
    [DataMember(Order = 4)]
    public double CenterFromFront { get; set; } = 15;
}
