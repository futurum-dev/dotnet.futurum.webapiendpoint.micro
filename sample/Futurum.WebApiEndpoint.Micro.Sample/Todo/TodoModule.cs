using Futurum.Microsoft.Extensions.DependencyInjection;

using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

public class TodoModule : IModule
{
    private readonly IConfiguration _configuration;

    public TodoModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Load(IServiceCollection services)
    {
        var configurationService = new TodoConfigurationService(_configuration);
        services.AddSingleton<ITodoConfigurationService>(configurationService);
        
        services.AddScoped(_ => new SqliteConnection(configurationService.GetConnectionString()));

        services.AddStartable<TodoStartable>();
    }
}