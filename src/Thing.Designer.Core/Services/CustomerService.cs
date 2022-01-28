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

internal class CustomerService : ICustomerService
{
    private readonly CurrentCustomerProvider _currentCustomerProvider;
    private readonly IMapper _mapper;

    public CustomerService(CurrentCustomerProvider currentCustomerProvider, IMapper mapper)
    {
        _currentCustomerProvider = currentCustomerProvider;
        _mapper = mapper;
    }

    public async Task<Result<GetCurrentCustomerResponse>> GetCurrentCustomer(CancellationToken cancellationToken)
    {
        var customer = await _currentCustomerProvider.GetOrCreateCurrentCustomer(cancellationToken);

        var response = _mapper.Map<GetCurrentCustomerResponse>(customer);

        return Result<GetCurrentCustomerResponse>.Success(response);
    }
}
