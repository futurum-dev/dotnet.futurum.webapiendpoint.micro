using Futurum.Microsoft.Extensions.DependencyInjection;

namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

public class BlogModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton<IBlogStorageBroker, BlogStorageBroker>();
    }
}