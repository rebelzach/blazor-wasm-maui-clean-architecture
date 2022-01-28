using AutoMapper;
using Thing.Designer.SolidModeling.Models;
using Thing.Domain.SolidModeling.Models;

namespace Thing.Designer.SolidModeling.MappingProfiles;

public class SolidModelDesignProfile : Profile
{
    public SolidModelDesignProfile()
    {
        CreateMap<HoleFeature, HoleFeatureDto>();
        CreateMap<SolidModelDesign, SolidModelDesignDto>();
        CreateMap<HoleFeatureDto, HoleFeature>();
        CreateMap<SolidModelDesignDto, SolidModelDesign>();
    }
}
