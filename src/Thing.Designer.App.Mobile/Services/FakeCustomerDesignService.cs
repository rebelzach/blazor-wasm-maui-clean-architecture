using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Thing.Designer.Core.Contracts.Models;
using Thing.Designer.Core.Contracts.Services;

namespace Thing.Designer.App.Mobile.Services;

public class FakeCustomerDesignService : ICustomerDesignService
{
    public Task<ContractResult<AddOrUpdateCustomerDesignResponse>> AddOrUpdateDesignAsync(AddOrUpdateCustomerDesignRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ContractResult<AddOrUpdateCustomerDesignResponse>>(null);
    }

    public Task<ContractResult<ListCustomerDesignsResponse>> ListDesignsAsync(ListCustomerDesignsRequest request, CancellationToken cancellationToken = default)
    {
        var designs = Enumerable.Range(1, 10)
            .Select(i => new CustomerDesignDto()
            {
                DesignId = Guid.NewGuid(),
                DesignName = $"Design {i}"
            })
            .ToList();
        var result = Result<ListCustomerDesignsResponse>.Success(new ListCustomerDesignsResponse(designs)).AsContractResult();
        return Task.FromResult(result);
    }
}
