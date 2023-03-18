using System.ComponentModel.DataAnnotations;

using Futurum.Core.Result;
using Futurum.Test.Result;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class DataAnnotationsValidationServiceTests
{
    public class Sync
    {
        public class WithNoValidation
        {
            [Fact]
            public void success()
            {
                var result = TestRunner(new NoValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            private static Result TestRunner(NoValidation instance)
            {
                var dataAnnotationsValidationService = new DataAnnotationsValidationService();

                return dataAnnotationsValidationService.Execute(instance);
            }
        }

        public class WithValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "The Name field is required.";

            [Fact]
            public void when_no_validation_errors_then_success()
            {
                var result = TestRunner(new HasValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            [Fact]
            public void when_validation_errors_then_failure()
            {
                var result = TestRunner(new HasValidation(null));

                result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage}'");
            }

            private static Result TestRunner(HasValidation hasValidation)
            {
                var dataAnnotationsValidationService = new DataAnnotationsValidationService();

                return dataAnnotationsValidationService.Execute(hasValidation);
            }
        }
    }

    public record NoValidation(string Name);

    public record HasValidation([property: Required] string Name);
}