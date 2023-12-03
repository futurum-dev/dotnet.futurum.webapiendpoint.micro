using Microsoft.AspNetCore.Routing;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointMetadataAttributeStrategyTests
{
    public class one_version
    {
        private const string ROUTE = "route";
        private const string TAG = "tag";

        public static readonly WebApiEndpointVersion V_DEFAULT = new(1, 0);
        public static readonly WebApiEndpointVersion V0_1 = new(0, 1);

        [Fact]
        public void all_populated()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new AllPopulatedWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(1);

            var metadata = metadatas.Single();

            metadata.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata.Tag.Should().Be(TAG);
            metadata.WebApiEndpointVersion.Should().Be(V0_1);
        }

        [Fact]
        public void version_missing()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new VersionMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(1);

            var metadata = metadatas.Single();

            metadata.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata.Tag.Should().Be(TAG);
            metadata.WebApiEndpointVersion.Should().Be(V_DEFAULT);
        }

        [Fact]
        public void tag_missing()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new TagMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(1);

            var metadata = metadatas.Single();

            metadata.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata.Tag.Should().Be(ROUTE);
            metadata.WebApiEndpointVersion.Should().Be(V0_1);
        }

        [Fact]
        public void tag_and_route_missing()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new TagAndRouteMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(1);

            var metadata = metadatas.Single();

            metadata.PrefixRoute.Should().Be($"v{{version:apiVersion}}/");
            metadata.Tag.Should().Be(string.Empty);
            metadata.WebApiEndpointVersion.Should().Be(V0_1);
        }

        [Fact]
        public void version_prefix_override()
        {
            const string versionPrefix = "v2";

            var configuration = new WebApiEndpointConfiguration(V_DEFAULT)
            {
                VersionPrefix = versionPrefix
            };

            var webApiEndpoint = new TagAndRouteMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(1);

            var metadata = metadatas.Single();

            metadata.PrefixRoute.Should().Be($"{versionPrefix}{{version:apiVersion}}/");
            metadata.Tag.Should().Be(string.Empty);
            metadata.WebApiEndpointVersion.Should().Be(V0_1);
        }

        [WebApiEndpoint(ROUTE, TAG)]
        [V0_1]
        public class AllPopulatedWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        [WebApiEndpoint(ROUTE, TAG)]
        public class VersionMissingWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        [WebApiEndpoint(ROUTE)]
        [V0_1]
        public class TagMissingWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        [WebApiEndpoint]
        [V0_1]
        public class TagAndRouteMissingWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        /// <inheritdoc />
        public class V0_1Attribute : WebApiEndpointVersionAttribute
        {
            public V0_1Attribute()
                : base(V0_1.MajorVersion, V0_1.MinorVersion)
            {
            }
        }
    }

    public class multiple_versions
    {
        private const string ROUTE = "route";
        private const string TAG = "tag";

        public static readonly WebApiEndpointVersion V_DEFAULT = new(1, 0);
        public static readonly WebApiEndpointVersion V0_1 = new(0, 1);
        public static readonly WebApiEndpointVersion V2_0 = new(2, 0);

        [Fact]
        public void all_populated()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new AllPopulatedWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(2);

            var metadata1 = metadatas.First();

            metadata1.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata1.Tag.Should().Be(TAG);
            metadata1.WebApiEndpointVersion.Should().Be(V0_1);

            var metadata2 = metadatas.Skip(1).First();

            metadata2.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata2.Tag.Should().Be(TAG);
            metadata2.WebApiEndpointVersion.Should().Be(V2_0);
        }

        [Fact]
        public void tag_missing()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new TagMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(2);

            var metadata1 = metadatas.First();

            metadata1.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata1.Tag.Should().Be(ROUTE);
            metadata1.WebApiEndpointVersion.Should().Be(V0_1);

            var metadata2 = metadatas.Skip(1).First();

            metadata2.PrefixRoute.Should().Be($"v{{version:apiVersion}}/{ROUTE}");
            metadata2.Tag.Should().Be(ROUTE);
            metadata2.WebApiEndpointVersion.Should().Be(V2_0);
        }

        [Fact]
        public void tag_and_route_missing()
        {
            var configuration = new WebApiEndpointConfiguration(V_DEFAULT);

            var webApiEndpoint = new TagAndRouteMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(2);

            var metadata1 = metadatas.First();

            metadata1.PrefixRoute.Should().Be($"v{{version:apiVersion}}/");
            metadata1.Tag.Should().Be(string.Empty);
            metadata1.WebApiEndpointVersion.Should().Be(V0_1);

            var metadata2 = metadatas.Skip(1).First();

            metadata2.PrefixRoute.Should().Be($"v{{version:apiVersion}}/");
            metadata2.Tag.Should().Be(string.Empty);
            metadata2.WebApiEndpointVersion.Should().Be(V2_0);
        }

        [Fact]
        public void version_prefix_override()
        {
            const string versionPrefix = "v2";

            var configuration = new WebApiEndpointConfiguration(V_DEFAULT)
            {
                VersionPrefix = versionPrefix
            };

            var webApiEndpoint = new TagAndRouteMissingWebApiEndpoint();

            var metadataAttributeStrategy = new WebApiEndpointMetadataAttributeStrategy();

            var metadatas = metadataAttributeStrategy.Get(configuration, webApiEndpoint);

            metadatas.Count().Should().Be(2);

            var metadata1 = metadatas.First();

            metadata1.PrefixRoute.Should().Be($"{versionPrefix}{{version:apiVersion}}/");
            metadata1.Tag.Should().Be(string.Empty);
            metadata1.WebApiEndpointVersion.Should().Be(V0_1);

            var metadata2 = metadatas.Skip(1).First();

            metadata2.PrefixRoute.Should().Be($"{versionPrefix}{{version:apiVersion}}/");
            metadata2.Tag.Should().Be(string.Empty);
            metadata2.WebApiEndpointVersion.Should().Be(V2_0);
        }

        [WebApiEndpoint(ROUTE, TAG)]
        [V0_1]
        [V2_0]
        public class AllPopulatedWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        [WebApiEndpoint(ROUTE)]
        [V0_1]
        [V2_0]
        public class TagMissingWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        [WebApiEndpoint]
        [V0_1]
        [V2_0]
        public class TagAndRouteMissingWebApiEndpoint : IWebApiEndpoint
        {
            public void Register(IEndpointRouteBuilder builder)
            {
            }
        }

        /// <inheritdoc />
        public class V0_1Attribute : WebApiEndpointVersionAttribute
        {
            public V0_1Attribute()
                : base(V0_1.MajorVersion, V0_1.MinorVersion)
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
    }
}