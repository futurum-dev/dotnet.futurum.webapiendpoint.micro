namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

public interface ITodoConfigurationService
{
    string GetConnectionString();
}

public class TodoConfigurationService : ITodoConfigurationService
{
    private readonly IConfiguration _configuration;

    public TodoConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString() =>
        _configuration.GetConnectionString("TodoDb")
        ?? _configuration["CONNECTION_STRING"]
        ?? "Data Source=todos.db;Cache=Shared";
}