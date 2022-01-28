using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thing.Designer.Core.InfrastructureContracts;

namespace Thing.Designer.Infrastructure.Local.Models;

public record CurrentApplicationUser(string? ApplicationUserId) : ICurrentApplicationUser;
