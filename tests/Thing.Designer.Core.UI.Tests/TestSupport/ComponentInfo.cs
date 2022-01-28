using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thing.Designer.Core.UI.Tests.TestSupport;

public record ComponentInfo(object? instance, int renderCount, int nodeCount, string markupLength);
