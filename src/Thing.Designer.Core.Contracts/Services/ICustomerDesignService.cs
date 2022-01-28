
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace Thing.Designer.Core.Contracts.Services;

[Service]
public interface ICustomerDesignService
{
    Task<ContractResult<AddOrUpdateCustomerDesignResponse>> AddOrUpdateDesignAsync(AddOrUpdateCustomerDesignRequest request, CancellationToken cancellationToken = default);
    Task<ContractResult<ListCustomerDesignsResponse>> ListDesignsAsync(ListCustomerDesignsRequest request, CancellationToken cancellationToken = default);
}
