namespace Futurum.WebApiEndpoint.Micro.Sample;

public static class WebApiEndpointVersions
{
    public static readonly WebApiEndpointVersion V0_1 = new(0, 1);

    public static readonly WebApiEndpointVersion V1_0 = new(1, 0);

    public static readonly WebApiEndpointVersion V2_0 = new(2, 0);

    public static readonly WebApiEndpointVersion V3_0 = new(3, 0);

    /// <inheritdoc />
    public class V0_1Attribute : WebApiEndpointVersionAttribute
    {
        public V0_1Attribute()
            : base(V0_1.MajorVersion, V0_1.MinorVersion)
        {
        }
    }

    /// <inheritdoc />
    public class V1_0Attribute : WebApiEndpointVersionAttribute
    {
        public V1_0Attribute()
            : base(V1_0.MajorVersion, V1_0.MinorVersion)
        {
        }
    }

    /// <inheritdoc />
    public class V2_0Attribute : WebApiEndpointVersionAttribute
    {
        public V2_0Attribute()
            : base(V2_0.MajorVersion, V2_0.MinorVersion)
        {
        }
    }

    /// <inheritdoc />
    public class V3_0Attribute : WebApiEndpointVersionAttribute
    {
        public V3_0Attribute()
            : base(V3_0.MajorVersion, V3_0.MinorVersion)
        {
        }
    }
}