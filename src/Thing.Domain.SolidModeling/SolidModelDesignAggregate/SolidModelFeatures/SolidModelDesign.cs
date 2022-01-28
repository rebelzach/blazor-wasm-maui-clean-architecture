using g3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thing.Domain.SolidModeling.Models;

public class SolidModelDesign
{
    public double Width { get; internal set; }
    public double Height { get; internal set; }
    public double Depth { get; internal set; }
    public List<IFeature> Features { get; internal set; } = new List<IFeature>();
}
