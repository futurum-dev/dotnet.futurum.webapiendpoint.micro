using FluentAssertions;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class ValidationResultErrorTests
{
    private const string Property1 = "Property1";
    private const string Property2 = "Property2";

    private const string ErrorMessage1 = "ErrorMessage1";
    private const string ErrorMessage2 = "ErrorMessage2";
    private const string ErrorMessage3 = "ErrorMessage3";

    [Fact]
    public void GetErrorStructureSafe()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        var resultError = validationErrors.ToResultError();

        var resultErrorStructure = resultError.GetErrorStructureSafe();
        resultErrorStructure.Message.Should().Be("Validation failure");
        resultErrorStructure.Children.Count().Should().Be(2);
        resultErrorStructure.Children.First().Message.Should().Be($"Validation failure for '{Property1}' with error : '{ErrorMessage1};{ErrorMessage3}'");
        resultErrorStructure.Children.Skip(1).First().Message.Should().Be($"Validation failure for '{Property2}' with error : '{ErrorMessage2}'");
    }

    [Fact]
    public void GetErrorStructure()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        var resultError = validationErrors.ToResultError();

        var resultErrorStructure = resultError.GetErrorStructure();
        resultErrorStructure.Message.Should().Be("Validation failure");
        resultErrorStructure.Children.Count().Should().Be(2);
        resultErrorStructure.Children.First().Message.Should().Be($"Validation failure for '{Property1}' with error : '{ErrorMessage1};{ErrorMessage3}'");
        resultErrorStructure.Children.Skip(1).First().Message.Should().Be($"Validation failure for '{Property2}' with error : '{ErrorMessage2}'");
    }

    [Fact]
    public void GetErrorStringSafe()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        var resultError = validationErrors.ToResultError();

        var resultErrorStructure = resultError.GetErrorStringSafe();
        resultErrorStructure.Should().Be($"Validation failure for '{Property1}' with error : '{ErrorMessage1};{ErrorMessage3}',Validation failure for '{Property2}' with error : '{ErrorMessage2}'");
    }

    [Fact]
    public void GetErrorString()
    {
        var validationErrors = new List<ValidationError>
        {
            new(Property1, new[] { ErrorMessage1, ErrorMessage3 }),
            new(Property2, new[] { ErrorMessage2 })
        };

        var resultError = validationErrors.ToResultError();

        var resultErrorStructure = resultError.GetErrorString();
        resultErrorStructure.Should().Be($"Validation failure for '{Property1}' with error : '{ErrorMessage1};{ErrorMessage3}',Validation failure for '{Property2}' with error : '{ErrorMessage2}'");
    }
}