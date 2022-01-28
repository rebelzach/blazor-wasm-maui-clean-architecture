
using Thing.Domain.CustomerDesignAggregate;

namespace Thing.Designer.Core.MappingProfiles;

public class CustomerDesignProfile : Profile
{
    public CustomerDesignProfile()
    {
        CreateMap<AddOrUpdateCustomerDesignRequest, CustomerDesign>()
            .ForMember(c => c.Id, opt => opt.MapFrom(r => r.DesignId));
        CreateMap<CustomerDesign, CustomerDesignDto>()
            .ForMember(c => c.DesignId, opt => opt.MapFrom(r => r.Id));
    }
}
