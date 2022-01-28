
using ProtoBuf;

namespace Thing.Designer.Core.Contracts.Models;

[ProtoContract]
[DataContract]
public record ListCustomerDesignsRequest()
{
    [DataMember(Order = 1)]
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
};
