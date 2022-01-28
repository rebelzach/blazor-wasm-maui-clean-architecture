
namespace Thing.Designer.Core.MappingProfiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, GetCurrentCustomerResponse>()
            .ForCtorParam(nameof(GetCurrentCustomerResponse.CustomerId), opt => opt.MapFrom(c => c.Id));
    }
}
