namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    /// <summary>
    /// Transforms a <typeparamref name="T"></typeparamref> to a <see cref="Ok{T}"/>.
    /// </summary>
    public static Ok<T> ToOk<T>(this T value) =>
        TypedResults.Ok(value);

    /// <summary>
    /// Transforms a <typeparamref name="T"></typeparamref> to a <see cref="Ok{T}"/>.
    /// </summary>
    public static Ok<T> ToOk<T>(HttpContext context, T value) =>
        TypedResults.Ok(value);
}
