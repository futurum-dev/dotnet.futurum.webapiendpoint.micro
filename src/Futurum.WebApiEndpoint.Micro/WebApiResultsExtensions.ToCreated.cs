using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Created ToCreated(HttpContext context) =>
        TypedResults.Created(context.Request.Path);

    public static Created<T> ToCreated<T>(HttpContext context, T value) =>
        TypedResults.Created(context.Request.Path, value);

    public static Func<HttpContext, T, Created<T>> ToCreated<T>(string uri) =>
        (_, value) => TypedResults.Created(uri, value);

    public static Func<HttpContext, T, Created<T>> ToCreated<T>(Uri uri) =>
        (_, value) => TypedResults.Created(uri, value);

    public static Func<HttpContext, Created> ToCreated(string uri) =>
        (_) => TypedResults.Created(uri);

    public static Func<HttpContext, Created> ToCreated(Uri uri) =>
        (_) => TypedResults.Created(uri);
}