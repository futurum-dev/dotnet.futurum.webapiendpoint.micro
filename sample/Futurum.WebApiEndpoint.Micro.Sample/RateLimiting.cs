using System.Net;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;

namespace Futurum.WebApiEndpoint.Micro.Sample;

public static class RateLimiting
{
    public static class SlidingWindow
    {
        public const string PolicyName = "SlidingWindowPolicy";

        public static readonly Action<RateLimiterOptions> Options = options =>
        {
            options.AddSlidingWindowLimiter(PolicyName,
                                            slidingWindowRateLimiterOptions =>
                                            {
                                                slidingWindowRateLimiterOptions.PermitLimit = 2;
                                                slidingWindowRateLimiterOptions.Window = TimeSpan.FromSeconds(5);
                                                slidingWindowRateLimiterOptions.SegmentsPerWindow = 2;
                                                slidingWindowRateLimiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                                                slidingWindowRateLimiterOptions.QueueLimit = 2;
                                            });

            ConfigureOnRejected(options);
        };
    }

    private static void ConfigureOnRejected(RateLimiterOptions options)
    {
        options.OnRejected = async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;

            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s)...", cancellationToken);
            }
            else
            {
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later...", cancellationToken);
            }
        };
    }
}