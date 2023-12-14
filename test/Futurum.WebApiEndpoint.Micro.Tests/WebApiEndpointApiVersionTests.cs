using Futurum.WebApiEndpoint.Micro.Generator;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointApiVersionTests
{
    public class WebApiEndpointNumberApiVersion
    {
        [Fact]
        public void check_equals()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");

            version1.Equals(version2).Should().BeTrue();
        }

        [Fact]
        public void check_equals_operator()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");

            (version1 == version2).Should().BeTrue();
        }

        [Fact]
        public void check_not_equals_operator()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(2.0, "alpha");

            (version1 != version2).Should().BeTrue();
        }

        [Fact]
        public void check_hashcode()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(1.0, "alpha");

            (version1.GetHashCode() == version2.GetHashCode()).Should().BeTrue();
        }
    }

    public class WebApiEndpointStringApiVersionVersion
    {
        [Fact]
        public void check_equals()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");

            version1.Equals(version2).Should().BeTrue();
        }

        [Fact]
        public void check_equals_operator()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");

            (version1 == version2).Should().BeTrue();
        }

        [Fact]
        public void check_not_equals_operator()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("2.0");

            (version1 != version2).Should().BeTrue();
        }

        [Fact]
        public void check_hashcode()
        {
            var version1 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");
            var version2 = new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion("1.0");

            (version1.GetHashCode() == version2.GetHashCode()).Should().BeTrue();
        }
    }
}
