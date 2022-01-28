using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Thing.Designer.Core.InfrastructureContracts;
using Thing.Designer.Infrastructure.Data;
using Thing.Designer.Infrastructure.Local.Models;
using Thing.Designer.Infrastructure.Services;
using Thing.SharedKernel.Interfaces;

namespace Thing.Designer.Infrastructure.Local;

public static class InfrastructureServiceCollectionExtensions
{
    public static void AddThingDesignerInfrastructure(
        this IServiceCollection services,
        string sqliteConnectionString,
        Action<IdentityBuilder>? configureIdentity,
        Action<IIdentityServerBuilder>? configureIdentityServer)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(sqliteConnectionString));

        services.AddInfrastructureServices(configureIdentity, configureIdentityServer);
    }

    /// <summary>
    /// Overload provided to support injecting a custom connection for testing
    /// </summary>
    public static void AddThingDesignerInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> configureContext)
    {
        services.AddDbContext<ApplicationDbContext>(configureContext);

        services.AddInfrastructureServices(null, null);
    }

    private static void AddInfrastructureServices(this IServiceCollection services, Action<IdentityBuilder>? configureIdentity, Action<IIdentityServerBuilder>? configureIdentityServer)
    {
        var identityBuilder = services
            .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        configureIdentity?.Invoke(identityBuilder);

        var identityServerBuilder = services.AddIdentityServer();

        configureIdentityServer?.Invoke(identityServerBuilder);
            //.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            //.AddProfileService<SampleProfileService>();

        services.AddSingleton<IGuidGenerator, GuidGenerator>();

        services.AddThingDesignerCoreServices();

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentApplicationUser>(sp =>
        {
            var userId = sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.User?.FindFirstValue("sub");
            return new CurrentApplicationUser(userId);
        });
    }
}

public class SampleProfileService : IProfileService
{
    private readonly HashSet<string> _allowedClaims = new HashSet<string>(
        new[]
        {
            "sub",
        });

    // this method adds claims that should go into the token to context.IssuedClaims
    public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var requestedClaimTypes = context.RequestedClaimTypes;
        var user = context.Subject;

        context.IssuedClaims.AddRange(
            user.Claims
                .Where(c => 
                    _allowedClaims.Contains(c.Type) &&
                    requestedClaimTypes.Contains(c.Type)));

        //// your implementation to retrieve the requested information
        //var claims = GetRequestedClaims(user, requestedClaimsTypes);
        //context.IssuedClaims.AddRange(claims);

        return Task.CompletedTask;
    }

    // this method allows to check if the user is still "enabled" per token request
    public virtual Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
