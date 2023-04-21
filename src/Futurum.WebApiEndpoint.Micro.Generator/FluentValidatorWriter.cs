using Futurum.WebApiEndpoint.Micro.Generator.Core;

namespace Futurum.WebApiEndpoint.Micro.Generator;

public static class FluentValidatorWriter
{
    public static string Write(string methodName, IEnumerable<FluentValidatorDatum> fluentValidatorData) =>
        WrapperSourceGeneratorWriter.Write(methodName, "RegisterFluentValidators",
                                           codeBuilder => Write(codeBuilder, fluentValidatorData),
                                           true);

    private static void Write(IndentedStringBuilder codeBuilder, IEnumerable<FluentValidatorDatum> fluentValidatorData)
    {
        foreach (var fluentValidatorDatum in fluentValidatorData)
        {
            codeBuilder.AppendLine($"serviceCollection.AddSingleton(typeof({fluentValidatorDatum.InterfaceType}), typeof({fluentValidatorDatum.ImplementationType}));");
        }
    }
}