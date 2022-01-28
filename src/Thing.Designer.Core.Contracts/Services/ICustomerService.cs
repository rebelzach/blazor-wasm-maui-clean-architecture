
namespace Thing.Designer.Core.Contracts.Services;

[ServiceContract]
public interface ICustomerService
{
    Task<Result<GetCurrentCustomerResponse>> GetCurrentCustomer(CancellationToken cancellationToken);
}
