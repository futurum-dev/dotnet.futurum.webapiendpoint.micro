using Futurum.Microsoft.Extensions.DependencyInjection;

using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

public class TodoStartable : IStartable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly ITodoConfigurationService _configurationService;

    public TodoStartable(IServiceProvider serviceProvider,
                         ILogger<TodoStartable> logger,
                         ITodoConfigurationService configurationService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configurationService = configurationService;
    }

    public void Start()
    {
        var connectionString = _configurationService.GetConnectionString();

        if (Environment.GetEnvironmentVariable("SUPPRESS_DB_INIT") != "true")
        {
            _logger.LogInformation("Ensuring database exists at connection string '{connectionString}'", connectionString);

            using var db = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SqliteConnection>();
            var sql = $"""
                  CREATE TABLE IF NOT EXISTS Todos (
                  {nameof(Todo.Id)} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                  {nameof(Todo.Title)} TEXT NOT NULL,
                  {nameof(Todo.IsComplete)} INTEGER DEFAULT 0 NOT NULL CHECK({nameof(Todo.IsComplete)} IN (0, 1))
                  );
               """;
            db.ExecuteAsync(sql).Wait();
        }
        else
        {
            Console.WriteLine($"Database initialization disabled for connection string '{connectionString}'");
        }
    }
}