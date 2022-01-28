
using Microsoft.Extensions.DependencyInjection;
using Thing.Designer.Core.Contracts.Services;
using Thing.Designer.Core.Services;
using Thing.Designer.Services;

namespace Thing.Designer;

public static class ThingDesignerServiceCollectionExtensions
{
    public static void AddThingDesignerCoreServices(this IServiceCollection services)
    {
        services.AddScoped<CurrentCustomerProvider>();
        services.AddScoped<ICustomerDesignService, CustomerDesignService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddAutoMapper(typeof(ThingDesignerServiceCollectionExtensions).Assembly);
    }
}
