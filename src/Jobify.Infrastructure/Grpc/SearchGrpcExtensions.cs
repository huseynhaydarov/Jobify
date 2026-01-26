using Grpc.Net.Client;
using MagicOnion.Client;
using SearchService.Contracts.Interfaces;

namespace Jobify.Infrastructure.Grpc;

public static class SearchGrpcExtensions
{
    public static IServiceCollection AddSearchGrpcClient(this IServiceCollection services)
    {
        services.AddSingleton<ISearchService>(sp =>
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212");

            return MagicOnionClient.Create<ISearchService>(channel);
        });

        return services;
    }
}
