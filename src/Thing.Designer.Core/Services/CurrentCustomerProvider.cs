using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Thing.Designer.Core.Contracts.Models;
using Thing.Designer.Core.Contracts.Services;
using Thing.Domain.CustomerAggregate;
using Thing.Domain.CustomerAggregate.Specifications;
using Thing.SharedKernel.Interfaces;

namespace Thing.Designer.Core.Services;

public class CurrentCustomerProvider
{
    private readonly IRepository<Customer> _customers;
    private readonly IMapper _mapper;
    private readonly ICurrentApplicationUser _currentApplicationUser;

    public CurrentCustomerProvider(IRepository<Customer> customers, IMapper mapper, ICurrentApplicationUser currentApplicationUser)
    {
        _customers = customers;
        _mapper = mapper;
        _currentApplicationUser = currentApplicationUser;
    }

    public async Task<Customer> GetOrCreateCurrentCustomer(CancellationToken cancellationToken)
    {
        var applicationUserId = _currentApplicationUser.ApplicationUserId ?? throw new NullReferenceException(nameof(_currentApplicationUser.ApplicationUserId));

        var customer = (await _customers.ListAsync(new CustomerApplicationUserIdSpec(applicationUserId), cancellationToken)).FirstOrDefault();

        if (customer is null)
        {
            customer = new Customer(applicationUserId);
            await _customers.AddAsync(customer, cancellationToken);
            await _customers.SaveChangesAsync();
        }

        return customer;
    }
}
