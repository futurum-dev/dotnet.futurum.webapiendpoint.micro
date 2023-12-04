namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

public record Id(long Value);

public static class IdExtensions
{
    public static Id ToId(this long id) =>
        new(id);
}
public record Blog(Id? Id, string Url);
