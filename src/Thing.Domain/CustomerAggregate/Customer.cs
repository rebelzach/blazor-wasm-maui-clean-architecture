
namespace Thing.Domain.CustomerAggregate;

public class Customer : BaseEntity<Guid>, IAggregateRoot
{
    public Customer(string applicationUserId)
    {
        ApplicationUserId = applicationUserId;
    }

    public string ApplicationUserId { get; private set; }
}
