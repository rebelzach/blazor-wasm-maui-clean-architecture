namespace Thing.Designer.Core.Contracts.Models;

[DataContract]
public class ListCustomerDesignsResponse
{
    [DataMember(Order = 1)] 
    public List<CustomerDesignDto> CustomerDesigns { get; set; } = new List<CustomerDesignDto>();

    private ListCustomerDesignsResponse()
    {
    }

    public ListCustomerDesignsResponse(List<CustomerDesignDto> customerDesigns)
    {
        CustomerDesigns = customerDesigns;
    }
}
