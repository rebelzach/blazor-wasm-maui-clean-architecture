using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thing.Designer.Core.InfrastructureContracts;

public interface ICurrentApplicationUser
{
    public string? ApplicationUserId { get; }
}
