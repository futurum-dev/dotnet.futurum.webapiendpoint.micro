using Microsoft.CodeAnalysis;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class SourceGeneratorExtensions
{
    public static WebApiEndpointApiVersion? ToWebApiEndpointApiVersion(this AttributeData attributeData)
    {
        if (attributeData.ConstructorArguments.Length >= 1 && attributeData.ConstructorArguments[0].Value is double versionNumber)
        {
            string? status = null;
            if (attributeData.ConstructorArguments.Length >= 2)
            {
                status = attributeData.ConstructorArguments[1].Value as string;
            }

            return new WebApiEndpointApiVersion.WebApiEndpointNumberApiVersion(versionNumber, status);
        }

        if (attributeData.ConstructorArguments.Length == 1 && attributeData.ConstructorArguments[0].Value is string versionString)
        {
            return new WebApiEndpointApiVersion.WebApiEndpointStringApiVersion(versionString);
        }

        return null;
    }
}
