using System.Diagnostics;
using ProtoBuf.Grpc;
using Thing.Designer.Core.Contracts.Services;
using Thing.Designer.Core.InfrastructureContracts;
using Thing.Designer.Core.Services;
using Thing.Domain.CustomerDesignAggregate;
using Thing.Domain.CustomerDesignAggregate.Specifications;
using Thing.SharedKernel.Interfaces;

namespace Thing.Designer.Services;

public class CustomerDesignService : ICustomerDesignService
{
    private readonly CurrentCustomerProvider _currentCustomerProvider;
    private readonly IRepository<CustomerDesign> _customerDesigns;
    private readonly IMapper _mapper;

    public CustomerDesignService(CurrentCustomerProvider currentCustomerProvider, IRepository<CustomerDesign> customerDesigns, IMapper mapper)
    {
        _currentCustomerProvider = currentCustomerProvider;
        _customerDesigns = customerDesigns;
        _mapper = mapper;
    }

    public async Task<ContractResult<AddOrUpdateCustomerDesignResponse>> AddOrUpdateDesignAsync(AddOrUpdateCustomerDesignRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await _currentCustomerProvider.GetOrCreateCurrentCustomer(cancellationToken);

        var customerDesign = _mapper.Map(request, new CustomerDesign(customer.Id));

        await _customerDesigns.AddAsync(customerDesign, cancellationToken);
        await _customerDesigns.SaveChangesAsync(cancellationToken);

        var response = new AddOrUpdateCustomerDesignResponse(customerDesign.Id);
        return Result<AddOrUpdateCustomerDesignResponse>.Success(response).AsContractResult();
    }

    public async Task<ContractResult<ListCustomerDesignsResponse>> ListDesignsAsync(ListCustomerDesignsRequest request, CancellationToken cancellationToken)
    {
        var customer = await _currentCustomerProvider.GetOrCreateCurrentCustomer(cancellationToken);
        var designs = await _customerDesigns.ListAsync(new CustomerDesignsCustomerIdSpec(customer.Id), cancellationToken);

        var designDtos = _mapper.Map<List<CustomerDesign>, List<CustomerDesignDto>>(designs!)!;
        var response = new ListCustomerDesignsResponse(designDtos);
        return Result<ListCustomerDesignsResponse>.Success(response).AsContractResult();
    }
}
