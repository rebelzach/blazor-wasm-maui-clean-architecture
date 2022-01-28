using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using Thing.Designer.Core.Contracts.Services;

namespace Thing.Designer.Infrastructure.WebClient;

public static class WebInfrastructureServiceCollectionExtensions
{
    public static void AddThingDesignerApiClient(this IServiceCollection serviceCollection, Action<IServiceProvider, ThingDesignerApiClientOptions> configure)
    {
        serviceCollection.AddScoped(services =>
        {
            var options = new ThingDesignerApiClientOptions();
            configure(services, options);
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, options.MessageHandler!));
            var channel = GrpcChannel.ForAddress(options.BaseUri!, new GrpcChannelOptions { HttpClient = httpClient, MaxReceiveMessageSize = null });
            return channel.CreateGrpcService<ICustomerDesignService>();
        });
    }
}

public class ThingDesignerApiClientOptions
{
    public string? BaseUri { get; set; }
    public HttpMessageHandler? MessageHandler { get; set; }
}
