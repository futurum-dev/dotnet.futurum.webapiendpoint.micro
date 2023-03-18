using System.ComponentModel.DataAnnotations;

using FluentValidation;

using Futurum.Core.Result;
using Futurum.Test.Result;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class ValidationServiceTests
{
    public class Sync
    {
        public class WithNoValidation
        {
            [Fact]
            public void success()
            {
                var result = TestRunner();

                result.ShouldBeSuccess();
            }

            private static Result TestRunner()
            {
                var validationService = new ValidationService<NoValidation>(new FluentValidationService<NoValidation>(Array.Empty<IValidator<NoValidation>>()),
                                                                            new DataAnnotationsValidationService());

                return validationService.Execute(new NoValidation(Guid.NewGuid().ToString()));
            }
        }

        public class WithOneValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            [Fact]
            public void when_no_validation_errors_then_success()
            {
                var result = TestRunner(new ValidatorSuccess(), new HasValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            [Fact]
            public void when_validation_errors_then_failure()
            {
                var result = TestRunner(new ValidatorFailure(), new HasValidation(null));

                result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage};The Name field is required.'");
            }

            private static Result TestRunner(IValidator<HasValidation> validator, HasValidation instance)
            {
                var validationService = new ValidationService<HasValidation>(new FluentValidationService<HasValidation>(new[] { validator }),
                                                                             new DataAnnotationsValidationService());

                return validationService.Execute(instance);
            }

            private class ValidatorSuccess : AbstractValidator<HasValidation>
            {
            }

            private class ValidatorFailure : AbstractValidator<HasValidation>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessage);
                }
            }
        }

        public class WithMoreValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            [Fact]
            public void when_no_validation_errors_then_success()
            {
                var result = TestRunner(new ValidatorSuccess(), new HasValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            [Fact]
            public void when_validation_errors_then_failure()
            {
                var result = TestRunner(new ValidatorFailure(), new HasValidation(null));

                var errorMessage = $"Validation failure for '{PropertyName}' with error : '{ErrorMessage};{ErrorMessage};The Name field is required.'";
                result.ShouldBeFailureWithError(errorMessage);
            }

            private static Result TestRunner(IValidator<HasValidation> validator, HasValidation instance)
            {
                var validationService = new ValidationService<HasValidation>(new FluentValidationService<HasValidation>(new[] { validator, validator }),
                                                                             new DataAnnotationsValidationService());

                return validationService.Execute(instance);
            }

            private class ValidatorSuccess : AbstractValidator<HasValidation>
            {
            }

            private class ValidatorFailure : AbstractValidator<HasValidation>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessage);
                }
            }
        }
    }

    public class Async
    {
        public class WithNoValidation
        {
            [Fact]
            public async Task success()
            {
                var result = await TestRunner();

                result.ShouldBeSuccess();
            }

            private static Task<Result> TestRunner()
            {
                var validationService = new ValidationService<NoValidation>(new FluentValidationService<NoValidation>(Array.Empty<IValidator<NoValidation>>()),
                                                                            new DataAnnotationsValidationService());

                return validationService.ExecuteAsync(new NoValidation(Guid.NewGuid().ToString()));
            }
        }

        public class WithOneValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            [Fact]
            public async Task when_no_validation_errors_then_success()
            {
                var result = await TestRunner(new ValidatorSuccess(), new HasValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            [Fact]
            public async Task when_validation_errors_then_failure()
            {
                var result = await TestRunner(new ValidatorFailure(), new HasValidation(null));

                result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage};The Name field is required.'");
            }

            private static Task<Result> TestRunner(IValidator<HasValidation> validator, HasValidation instance)
            {
                var validationService = new ValidationService<HasValidation>(new FluentValidationService<HasValidation>(new[] { validator }),
                                                                             new DataAnnotationsValidationService());

                return validationService.ExecuteAsync(instance);
            }

            private class ValidatorSuccess : AbstractValidator<HasValidation>
            {
            }

            private class ValidatorFailure : AbstractValidator<HasValidation>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessage);
                }
            }
        }

        public class WithMoreValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            [Fact]
            public async Task when_no_validation_errors_then_success()
            {
                var result = await TestRunner(new ValidatorSuccess(), new HasValidation(Guid.NewGuid().ToString()));

                result.ShouldBeSuccess();
            }

            [Fact]
            public async Task when_validation_errors_then_failure()
            {
                var result = await TestRunner(new ValidatorFailure(), new HasValidation(null));

                var errorMessage = $"Validation failure for '{PropertyName}' with error : '{ErrorMessage};{ErrorMessage};The Name field is required.'";
                result.ShouldBeFailureWithError(errorMessage);
            }

            private static Task<Result> TestRunner(IValidator<HasValidation> validator, HasValidation instance)
            {
                var validationService = new ValidationService<HasValidation>(new FluentValidationService<HasValidation>(new[] { validator, validator }),
                                                                             new DataAnnotationsValidationService());

                return validationService.ExecuteAsync(instance);
            }

            private class ValidatorSuccess : AbstractValidator<HasValidation>
            {
            }

            private class ValidatorFailure : AbstractValidator<HasValidation>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessage);
                }
            }
        }
    }

    public record NoValidation(string Name);

    public record HasValidation([property: Required] string Name);
}