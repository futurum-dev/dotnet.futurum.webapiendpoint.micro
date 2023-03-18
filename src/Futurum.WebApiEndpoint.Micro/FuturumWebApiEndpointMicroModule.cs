using Futurum.Microsoft.Extensions.DependencyInjection;

namespace Futurum.WebApiEndpoint.Micro;

public class FuturumWebApiEndpointMicroModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton(typeof(IFluentValidationService<>), typeof(FluentValidationService<>));
        services.AddSingleton<IDataAnnotationsValidationService, DataAnnotationsValidationService>();
        services.AddSingleton(typeof(IValidationService<>), typeof(ValidationService<>));
    }
}