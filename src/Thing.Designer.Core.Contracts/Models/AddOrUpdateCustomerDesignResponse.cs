
namespace Thing.Designer.Core.Contracts.Models;

[DataContract]
public class AddOrUpdateCustomerDesignResponse
{
    private AddOrUpdateCustomerDesignResponse() { } // For GRPC

    public AddOrUpdateCustomerDesignResponse(Guid id)
    {
        Id = id;
    }

    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}
