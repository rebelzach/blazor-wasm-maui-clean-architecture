
namespace Thing.Domain.CustomerDesignAggregate;

public class CustomerDesign : BaseEntity<Guid>, IAggregateRoot
{
    public CustomerDesign(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
    public string? DesignName { get; }
}
