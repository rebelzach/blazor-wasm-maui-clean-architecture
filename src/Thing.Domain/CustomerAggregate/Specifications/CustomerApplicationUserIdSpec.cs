using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Thing.Domain.CustomerAggregate.Specifications;

public class CustomerApplicationUserIdSpec : Specification<Customer>
{
    public CustomerApplicationUserIdSpec(string applicationUserId)
    {
        Query.Where(customer => customer.ApplicationUserId == applicationUserId);
    }
}
