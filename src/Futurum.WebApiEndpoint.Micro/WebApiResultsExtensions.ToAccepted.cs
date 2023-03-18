using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Accepted ToAccepted(HttpContext context) =>
        TypedResults.Accepted(context.Request.Path);

    public static Accepted<T> ToAccepted<T>(HttpContext context, T value) =>
        TypedResults.Accepted(context.Request.Path, value);

    public static Func<HttpContext, T, Accepted<T>> ToAccepted<T>(string uri) =>
        (_, value) => TypedResults.Accepted(uri, value);

    public static Func<HttpContext, T, Accepted<T>> ToAccepted<T>(Uri uri) =>
        (_, value) => TypedResults.Accepted(uri, value);

    public static Func<HttpContext, Accepted> ToAccepted(string uri) =>
        (_) => TypedResults.Accepted(uri);

    public static Func<HttpContext, Accepted> ToAccepted(Uri uri) =>
        (_) => TypedResults.Accepted(uri);
}