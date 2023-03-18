namespace Futurum.WebApiEndpoint.Micro.Sample.Addition;

public static class AdditionMapper
{
    public static AdditionDto Map(Addition domain) =>
        new(domain.Name);
}