namespace Futurum.WebApiEndpoint.Micro;

public record DataCollectionDto<TData>(ICollection<TData> Data)
{
    /// <summary>
    /// Data count
    /// </summary>
    public long Count => Data.Count;
}

public static class DataCollectionExtensions
{
    public static DataCollectionDto<TDto> ToDataCollectionDto<TDto>(this IEnumerable<TDto> data) =>
        new(data.ToList());

    public static DataCollectionDto<TDto> ToDataCollectionDto<TDto>(this ICollection<TDto> data) =>
        new(data);
}