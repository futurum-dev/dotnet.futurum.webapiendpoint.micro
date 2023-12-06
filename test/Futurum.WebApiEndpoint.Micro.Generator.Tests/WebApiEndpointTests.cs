using System.Linq;

using FluentAssertions;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Tests;

public class WebApiEndpointTests
{
    [Fact]
    public void check()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWebApiEndpointsForFuturumWebApiEndpointMicroGeneratorTests();

        var serviceDescriptor = serviceCollection.Where(x => x.ServiceType == typeof(IWebApiEndpoint));

        serviceDescriptor.Count().Should().Be(1);
        serviceDescriptor.First().ImplementationType.Should().Be(typeof(WebApiEndpoint));
        serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}

[WebApiEndpoint("api", "test")]
public partial class WebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {

    }
}
