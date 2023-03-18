using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro.Sample.Blog;
using Futurum.WebApiEndpoint.Micro.Sample.Todo;

namespace Futurum.WebApiEndpoint.Micro.Sample;

public class ApplicationModule : IModule
{
    private readonly IConfiguration _configuration;

    public ApplicationModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Load(IServiceCollection services)
    {
        services.RegisterModule<BlogModule>();
        services.RegisterModule(new TodoModule(_configuration));
    }
}