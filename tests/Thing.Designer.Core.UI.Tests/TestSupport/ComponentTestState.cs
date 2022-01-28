using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;

namespace Thing.Designer.Core.UI.Tests.TestSupport;

public record ComponentTestState(IRenderedFragment Component, object? State = null);
