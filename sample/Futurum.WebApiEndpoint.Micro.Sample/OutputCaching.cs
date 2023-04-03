using Microsoft.AspNetCore.OutputCaching;

namespace Futurum.WebApiEndpoint.Micro.Sample;

public static class OutputCaching
{
    public static IServiceCollection AddOutputCaches(this IServiceCollection services)
    {
        services.AddOutputCache(options => { ExpiryIn10Seconds.Options(options); });

        return services;
    }

    public static class ExpiryIn10Seconds
    {
        public const string Policy = "ExpiryIn10SecondsOutputCachingPolicy";

        public static readonly Action<OutputCacheOptions> Options = options =>
        {
            options.AddPolicy(Policy, outputCachePolicyBuilder => { outputCachePolicyBuilder.Expire(TimeSpan.FromSeconds(10)); });
        };
    }
}