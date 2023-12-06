namespace Futurum.WebApiEndpoint.Micro.Sample.Blog;

public interface IBlogStorageBroker
{
    Task<IEnumerable<Blog>> GetAsync();
    Task<Blog> GetByIdAsync(Id id);

    Task<Blog> AddAsync(Blog blog);

    Task<Blog> UpdateAsync(Blog blog);

    Task DeleteAsync(Id id);
}

public class BlogStorageBroker : IBlogStorageBroker
{
    private readonly List<Blog> _items = new();

    public async Task<IEnumerable<Blog>> GetAsync() =>
        _items.AsReadOnly().AsEnumerable();

    public async Task<Blog> GetByIdAsync(Id id)
    {
        try
        {
            var existingBlog = _items.Single(x => x.Id == id);

            return existingBlog;
        }
        catch (Exception exception)
        {
            throw new KeyNotFoundException($"Unable to find {nameof(Blog)} with Id : '{id}'", exception);
        }
    }

    public async Task<Blog> AddAsync(Blog blog)
    {
        var newBlog = blog with
        {
            Id = ((long)_items.Count).ToId()
        };

        _items.Add(newBlog);

        return newBlog;
    }

    public async Task<Blog> UpdateAsync(Blog blog)
    {
        try
        {
            var existingBlog = _items.Single(x => x.Id == blog.Id);

            _items.Remove(existingBlog);
            _items.Add(blog);

            return blog;
        }
        catch (Exception exception)
        {
            throw new KeyNotFoundException($"Unable to find {nameof(Blog)} with Id : '{blog.Id}'", exception);
        }
    }

    public async Task DeleteAsync(Id id)
    {
        try
        {
            var existingBlog = _items.Single(x => x.Id == id);

            _items.Remove(existingBlog);
        }
        catch (Exception exception)
        {
            throw new KeyNotFoundException($"Unable to find {nameof(Blog)} with Id : '{id}'", exception);
        }
    }
}
