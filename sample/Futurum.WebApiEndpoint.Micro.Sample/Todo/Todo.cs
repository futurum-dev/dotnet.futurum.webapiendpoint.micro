using System.ComponentModel.DataAnnotations;
using System.Data;

using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

sealed class Todo : IDataReaderMapper<Todo>
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }

    public static Todo Map(SqliteDataReader dataReader)
    {
        return !dataReader.HasRows
            ? new()
            : new()
            {
                Id = dataReader.GetInt32(nameof(Id)),
                Title = dataReader.GetString(nameof(Title)),
                IsComplete = dataReader.GetBoolean(nameof(IsComplete))
            };
    }
}