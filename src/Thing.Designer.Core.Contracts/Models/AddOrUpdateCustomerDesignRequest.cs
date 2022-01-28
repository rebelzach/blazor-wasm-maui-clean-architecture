using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Thing.Designer.Core.Contracts.Models;

[ProtoContract]
public record AddOrUpdateCustomerDesignRequest
{
    public AddOrUpdateCustomerDesignRequest() { }

    public AddOrUpdateCustomerDesignRequest(Guid designId)
    {
        DesignId = designId;
    }

    [ProtoMember(1)]
    public Guid DesignId { get; set; }
    [ProtoMember(2)]
    public double Width { get; set; }
    [ProtoMember(3)]
    public double Height { get; set; }
    [ProtoMember(4)]
    public double Depth { get; set; }
}
