namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

public static class BlogMapper
{
    public static BlogDto MapToDto(Blog domain) =>
        new(domain.Id.GetValueOrDefault(x => x.Value, 0), domain.Url);
}