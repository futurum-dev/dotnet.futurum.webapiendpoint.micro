namespace Futurum.WebApiEndpoint.Micro.Sample;

public static class WebApiEndpointVersions
{
    public static class V0_1
    {
        public const double Number = 0.1d;
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Number);
    }

    public static class V1_0
    {
        public const double Number = 1.0d;
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Number);
    }

    public static class V2_0
    {
        public const double Number = 2.0d;
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Number);
    }

    public static class V3_0
    {
        public const double Number = 3.0d;
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Number);
    }

    public static class V4_0_Alpha
    {
        public const double Number = 4.0d;
        public const string Status = "alpha";
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Number, Status);
    }

    public static class V1_20_Beta
    {
        public const string Text = "1.20-beta";
        public static readonly WebApiEndpointVersion Version = WebApiEndpointVersion.Create(Text);
    }
}
