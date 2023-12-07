namespace Futurum.WebApiEndpoint.Micro.Sample;

public static class WebApiEndpointVersions
{
    public static class V0_1
    {
        public const int Major = 0;
        public const int Minor = 1;
        public static readonly WebApiEndpointVersion Version = new(Major, Minor);
    }

    public static class V1_0
    {
        public const int Major = 1;
        public const int Minor = 0;
        public static readonly WebApiEndpointVersion Version = new(Major, Minor);
    }

    public static class V2_0
    {
        public const int Major = 2;
        public const int Minor = 0;
        public static readonly WebApiEndpointVersion Version = new(Major, Minor);
    }

    public static class V3_0
    {
        public const int Major = 3;
        public const int Minor = 0;
        public static readonly WebApiEndpointVersion Version = new(Major, Minor);
    }
}
