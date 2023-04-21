using System.Linq;

using FluentAssertions;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.WebApiEndpoint.Micro.Generator.Tests;

public class FluentValidatorTests
{
    [Fact]
    public void check()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWebApiEndpointsForFuturumWebApiEndpointMicroGeneratorTests();

        var serviceDescriptor = serviceCollection.Where(x => x.ServiceType == typeof(IValidator<Dto>));

        serviceDescriptor.Count().Should().Be(1);
        serviceDescriptor.First().ImplementationType.Should().Be(typeof(Validator));
        serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    public record Dto(string Name);
        
    public class Validator : AbstractValidator<Dto>
    {
    }
}