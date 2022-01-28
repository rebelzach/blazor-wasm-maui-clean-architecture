using ProtoBuf;

namespace Thing.Designer.Core.Contracts.Models;

[ProtoContract]
[DataContract]
public class CustomerDesignDto
{
    [DataMember(Order = 1)] 
    public Guid DesignId { get; set; }

    [DataMember(Order = 2)] 
    public string? DesignName { get; set; }
}
