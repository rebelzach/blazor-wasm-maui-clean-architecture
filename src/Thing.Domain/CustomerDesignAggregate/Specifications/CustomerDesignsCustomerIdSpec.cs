using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Thing.Domain.CustomerDesignAggregate;

namespace Thing.Domain.CustomerDesignAggregate.Specifications;

public class CustomerDesignsCustomerIdSpec : Specification<CustomerDesign>
{
    public CustomerDesignsCustomerIdSpec(Guid customerId)
    {
        Query.Where(customer => customer.CustomerId == customerId);
    }
}
