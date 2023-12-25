using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiOpenApiVersionConfigurationServiceTests
{
    [Fact]
    public void when_IsDeprecated()
    {
        var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
        {
            OpenApi = new()
            {
                DefaultInfo = new()
                {
                    Title = "",
                }
            }
        };
        var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

        var apiVersionDescription = new ApiVersionDescription(new ApiVersion(1, 0), string.Empty, true, null);

        var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

        openApiInfo.Description.Should().Be(" This API version has been deprecated.");
    }

    [Fact]
    public void when_SunsetPolicy()
    {
        var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
        {
            OpenApi = new()
            {
                DefaultInfo = new()
                {
                    Title = "",
                }
            }
        };
        var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

        var linkHeaderValue = new LinkHeaderValue(new Uri("https://www.google.com"), "sunset")
        {
            Title = "Test",
            Type = "text/html"
        };
        var sunsetPolicy = new SunsetPolicy(new DateTime(2021, 1, 1), linkHeaderValue);

        var apiVersionDescription = new ApiVersionDescription(new ApiVersion(1, 0), string.Empty, false, sunsetPolicy);

        var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

        openApiInfo.Description.Should().Be($" The API will be sunset on 01/01/2021.{Environment.NewLine}{Environment.NewLine}Test: https://www.google.com");
    }

    public class VersionOverrides
    {
        public class Title
        {
            [Fact]
            public void Default()
            {
                var titleDefault = Guid.NewGuid().ToString();
                var titleV3 = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = titleDefault,
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = titleV3
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Title.Should().Be(titleDefault);
            }

            [Fact]
            public void Override()
            {
                var apiVersion = new ApiVersion(2, 0);

                var titleVersionOverride = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = "",
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = titleVersionOverride
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Title.Should().Be(titleVersionOverride);
            }
        }

        public class Description
        {
            [Fact]
            public void Default()
            {
                var descriptionDefault = Guid.NewGuid().ToString();
                var descriptionV3 = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            Description = descriptionDefault,
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Description = descriptionV3
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Description.Should().Be(descriptionDefault);
            }

            [Fact]
            public void Override()
            {
                var apiVersion = new ApiVersion(2, 0);

                var descriptionVersionOverride = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Description = descriptionVersionOverride
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Description.Should().Be(descriptionVersionOverride);
            }
        }

        public class TermsOfService
        {
            [Fact]
            public void Default()
            {
                var termsOfServiceDefault = new Uri("https://www.google.com");
                var termsOfServiceV3 = new Uri("https://www.bing.com");

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            TermsOfService = termsOfServiceDefault,
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    TermsOfService = termsOfServiceV3
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.TermsOfService.Should().Be(termsOfServiceDefault);
            }

            [Fact]
            public void Override()
            {
                var apiVersion = new ApiVersion(2, 0);

                var termsOfServiceVersionOverride = new Uri("https://www.bing.com");

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    TermsOfService = termsOfServiceVersionOverride
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.TermsOfService.Should().Be(termsOfServiceVersionOverride);
            }
        }

        public class Contact
        {
            [Fact]
            public void Default()
            {
                var contactDefault = new OpenApiContact
                {
                    Name = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.google.com")
                };
                var contactV3 = new OpenApiContact
                {
                    Name = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.bing.com")
                };

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            Contact = contactDefault,
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Contact = contactV3
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Contact.Should().BeEquivalentTo(contactDefault);
            }

            [Fact]
            public void Override()
            {
                var apiVersion = new ApiVersion(2, 0);

                var contactVersionOverride = new OpenApiContact
                {
                    Name = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.bing.com")
                };

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Contact = contactVersionOverride
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Contact.Should().BeEquivalentTo(contactVersionOverride);
            }
        }

        public class License
        {
            [Fact]
            public void Default()
            {
                var licenseDefault = new OpenApiLicense
                {
                    Name = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.google.com")
                };
                var licenseV3 = new OpenApiLicense
                {
                    Name = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.bing.com")
                };

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            License = licenseDefault,
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    License = licenseV3
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.License.Should().BeEquivalentTo(licenseDefault);
            }

            [Fact]
            public void Override()
            {
                var apiVersion = new ApiVersion(2, 0);

                var licenseVersionOverride = new OpenApiLicense
                {
                    Name = Guid.NewGuid().ToString(),
                    Url = new Uri("https://www.bing.com")
                };

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    License = licenseVersionOverride
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.License.Should().BeEquivalentTo(licenseVersionOverride);
            }
        }

        public class Extensions
        {
            [Fact]
            public void Default()
            {
                var defaultKey = Guid.NewGuid().ToString();
                var defaultValue = Guid.NewGuid().ToString();

                var versionedKey = Guid.NewGuid().ToString();
                var versionedValue = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            Extensions =
                            {
                                { defaultKey, new OpenApiString(defaultValue) }
                            },
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                new ApiVersion(3, 0),
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Extensions =
                                    {
                                        { versionedKey, new OpenApiString(versionedValue) }
                                    }
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(new ApiVersion(2, 0), string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Extensions.Should().BeEquivalentTo(new Dictionary<string, IOpenApiExtension>
                {
                    { defaultKey, new OpenApiString(defaultValue) }
                });
            }

            [Fact]
            public void Override_Merge()
            {
                var apiVersion = new ApiVersion(2, 0);

                var defaultKey = Guid.NewGuid().ToString();
                var defaultValue = Guid.NewGuid().ToString();

                var versionedKey = Guid.NewGuid().ToString();
                var versionedValue = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            Extensions =
                            {
                                { defaultKey, new OpenApiString(defaultValue) }
                            },
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Extensions =
                                    {
                                        { versionedKey, new OpenApiString(versionedValue) }
                                    }
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Extensions.Should().BeEquivalentTo(new Dictionary<string, IOpenApiExtension>
                {
                    { defaultKey, new OpenApiString(defaultValue) },
                    { versionedKey, new OpenApiString(versionedValue) }
                });
            }

            [Fact]
            public void Override_Replace()
            {
                var apiVersion = new ApiVersion(2, 0);

                var key = Guid.NewGuid().ToString();
                var defaultValue = Guid.NewGuid().ToString();

                var versionedValue = Guid.NewGuid().ToString();

                var webApiEndpointConfiguration = new WebApiEndpointConfiguration(WebApiEndpointVersion.Create(1.0d))
                {
                    OpenApi = new()
                    {
                        DefaultInfo = new()
                        {
                            Title = Guid.NewGuid().ToString(),
                            Extensions =
                            {
                                { key, new OpenApiString(defaultValue) }
                            },
                        },
                        VersionedOverrideInfo =
                        {
                            {
                                apiVersion,
                                new WebApiEndpointOpenApiInfo
                                {
                                    Title = Guid.NewGuid().ToString(),
                                    Extensions =
                                    {
                                        { key, new OpenApiString(versionedValue) }
                                    }
                                }
                            }
                        }
                    }
                };
                var webApiOpenApiVersionConfigurationService = new WebApiOpenApiVersionConfigurationService(webApiEndpointConfiguration);

                var apiVersionDescription = new ApiVersionDescription(apiVersion, string.Empty, false, null);

                var openApiInfo = webApiOpenApiVersionConfigurationService.CreateOpenApiInfo(apiVersionDescription);

                openApiInfo.Extensions.Should().BeEquivalentTo(new Dictionary<string, IOpenApiExtension>
                {
                    { key, new OpenApiString(versionedValue) }
                });
            }
        }
    }
}
