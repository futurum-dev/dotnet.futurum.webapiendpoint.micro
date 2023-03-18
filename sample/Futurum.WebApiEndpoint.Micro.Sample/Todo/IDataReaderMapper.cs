using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

public interface IDataReaderMapper<T>
{
    static abstract T Map(SqliteDataReader dataReader);
}
