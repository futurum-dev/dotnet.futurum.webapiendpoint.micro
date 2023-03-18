using FluentValidation;

using Futurum.Core.Result;
using Futurum.Test.Result;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class FluentValidationServiceTests
{
    public class Sync
    {
        public class NoValidation
        {
            private record Request;

            [Fact]
            public void success()
            {
                var result = TestRunner();

                result.ShouldBeSuccess();
            }

            private static Result TestRunner()
            {
                var fluentValidationService = new FluentValidationService<Request>(Array.Empty<IValidator<Request>>());

                return fluentValidationService.Execute(new Request());
            }
        }

        public class OneValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            private record Request(string Name);

            [Fact]
            public void when_no_validation_errors_then_success()
            {
                var result = TestRunner(new ValidatorSuccess());

                result.ShouldBeSuccess();
            }

            [Fact]
            public void when_validation_errors_then_failure()
            {
                var result = TestRunner(new ValidatorFailure());

                result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage}'");
            }

            private static Result TestRunner(IValidator<Request> validator)
            {
                var fluentValidationService = new FluentValidationService<Request>(new[] { validator });

                return fluentValidationService.Execute(new Request(Guid.NewGuid().ToString()));
            }

            private class ValidatorSuccess : AbstractValidator<Request>
            {
            }

            private class ValidatorFailure : AbstractValidator<Request>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
                }
            }
        }

        public class MoreValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            private record Request(string Name);

            [Fact]
            public void when_no_validation_errors_then_success()
            {
                var result = TestRunner(new ValidatorSuccess());

                result.ShouldBeSuccess();
            }

            [Fact]
            public void when_validation_errors_then_failure()
            {
                var result = TestRunner(new ValidatorFailure());

                var errorMessage = $"Validation failure for '{PropertyName}' with error : '{ErrorMessage};{ErrorMessage}'";
                result.ShouldBeFailureWithError(errorMessage);
            }

            private static Result TestRunner(IValidator<Request> validator)
            {
                var fluentValidationService = new FluentValidationService<Request>(new[] { validator, validator });

                return fluentValidationService.Execute(new Request(Guid.NewGuid().ToString()));
            }

            private class ValidatorSuccess : AbstractValidator<Request>
            {
            }

            private class ValidatorFailure : AbstractValidator<Request>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
                }
            }
        }
    }

    public class Async
    {
        public class NoValidation
        {
            private record Request;

            [Fact]
            public async Task success()
            {
                var result = await TestRunner();

                result.ShouldBeSuccess();
            }

            private static Task<Result> TestRunner()
            {
                var fluentValidationService = new FluentValidationService<Request>(Array.Empty<IValidator<Request>>());

                return fluentValidationService.ExecuteAsync(new Request());
            }
        }

        public class OneValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            private record Request(string Name);

            [Fact]
            public async Task when_no_validation_errors_then_success()
            {
                var result = await TestRunner(new ValidatorSuccess());

                result.ShouldBeSuccess();
            }

            [Fact]
            public async Task when_validation_errors_then_failure()
            {
                var result = await TestRunner(new ValidatorFailure());

                result.ShouldBeFailureWithError($"Validation failure for '{PropertyName}' with error : '{ErrorMessage}'");
            }

            private static Task<Result> TestRunner(IValidator<Request> validator)
            {
                var fluentValidationService = new FluentValidationService<Request>(new[] { validator });

                return fluentValidationService.ExecuteAsync(new Request(Guid.NewGuid().ToString()));
            }

            private class ValidatorSuccess : AbstractValidator<Request>
            {
            }

            private class ValidatorFailure : AbstractValidator<Request>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
                }
            }
        }

        public class MoreValidation
        {
            private const string PropertyName = "Name";
            private const string ErrorMessage = "ErrorMessage";

            private record Request(string Name);

            [Fact]
            public async Task when_no_validation_errors_then_success()
            {
                var result = await TestRunner(new ValidatorSuccess());

                result.ShouldBeSuccess();
            }

            [Fact]
            public async Task when_validation_errors_then_failure()
            {
                var result = await TestRunner(new ValidatorFailure());

                var errorMessage = $"Validation failure for '{PropertyName}' with error : '{ErrorMessage};{ErrorMessage}'";
                result.ShouldBeFailureWithError(errorMessage);
            }

            private static Task<Result> TestRunner(IValidator<Request> validator)
            {
                var fluentValidationService = new FluentValidationService<Request>(new[] { validator, validator });

                return fluentValidationService.ExecuteAsync(new Request(Guid.NewGuid().ToString()));
            }

            private class ValidatorSuccess : AbstractValidator<Request>
            {
            }

            private class ValidatorFailure : AbstractValidator<Request>
            {
                public ValidatorFailure()
                {
                    RuleFor(x => x.Name).Empty().WithMessage(ErrorMessage);
                }
            }
        }
    }
}