using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thing.Domain.SolidModeling.Models;

public class HoleFeature : IFeature
{
    public double Diameter { get; internal set; } = 10;
    public double Depth { get; internal set; } = 10;
    public double CenterFromLeft { get; internal set; } = 15;
    public double CenterFromFront { get; internal set; } = 15;
}
