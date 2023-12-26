using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class ExceptionToProblemDetailsMapperServiceTests
{
    public class default_exception
    {
        [Fact]
        public void without_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.OverrideDefault(ExceptionToProblemDetailsMapperService.DefaultException);
            var exception = new Exception("Test exception message");
            var requestPath = "/test";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath));

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be(exception.Message);
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest));
        }

        [Fact]
        public void with_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.OverrideDefault(ExceptionToProblemDetailsMapperService.DefaultException);
            var exception = new Exception("Test exception message");
            var requestPath = "/test";
            var errorMessage = "Test error message";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath), errorMessage);

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be($"{errorMessage};{exception.Message}");
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest));
        }
    }

    public class KeyNotFoundException_exception
    {
        [Fact]
        public void without_error_message()
        {
            var exception = new KeyNotFoundException("Test exception message");
            var requestPath = "/test";

            var problemDetails = new ExceptionToProblemDetailsMapperService().Map(exception, CreateHttpContext(requestPath));

            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be(exception.Message);
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
            problemDetails.Title.Should().Be("Key Not Found");
        }

        [Fact]
        public void with_error_message()
        {
            var exception = new KeyNotFoundException("Test exception message");
            var requestPath = "/test";
            var errorMessage = "Test error message";

            var problemDetails = new ExceptionToProblemDetailsMapperService().Map(exception, CreateHttpContext(requestPath), errorMessage);

            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be($"{errorMessage};{exception.Message}");
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
            problemDetails.Title.Should().Be("Key Not Found");
        }
    }

    public class custom_default_exception
    {
        [Fact]
        public void without_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.OverrideDefault(DefaultException);
            var exception = new Exception("Test exception message");
            var requestPath = "/test";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath));

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be(exception.Message);
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError));
        }

        [Fact]
        public void with_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.OverrideDefault(DefaultException);
            var exception = new Exception("Test exception message");
            var requestPath = "/test";
            var errorMessage = "Test error message";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath), errorMessage);

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be($"{errorMessage};{exception.Message}");
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError));
        }

        private static ProblemDetails DefaultException(Exception exception, HttpContext context, string? errorMessage) =>
            new()
            {
                Detail = !string.IsNullOrEmpty(errorMessage) ? $"{errorMessage};{exception.Message}" : exception.Message,
                Instance = context.Request.Path,
                Status = StatusCodes.Status500InternalServerError,
                Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError)
            };
    }

    public class add_custom_exception
    {
        [Fact]
        public void without_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.Add<CustomException>(DefaultException);
            var exception = new CustomException("Test exception message");
            var requestPath = "/test";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath));

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be($"Processing-{exception.Message}");
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status102Processing);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status102Processing));
        }

        [Fact]
        public void with_error_message()
        {
            // Arrange
            var exceptionToProblemDetailsMapperService = new ExceptionToProblemDetailsMapperService();
            exceptionToProblemDetailsMapperService.Add<CustomException>(DefaultException);
            var exception = new CustomException("Test exception message");
            var requestPath = "/test";
            var errorMessage = "Test error message";

            // Act
            var problemDetails = exceptionToProblemDetailsMapperService.Map(exception, CreateHttpContext(requestPath), errorMessage);

            // Assert
            problemDetails.Should().NotBeNull();
            problemDetails.Detail.Should().Be($"Processing-{errorMessage};{exception.Message}");
            problemDetails.Instance.Should().Be(requestPath);
            problemDetails.Status.Should().Be(StatusCodes.Status102Processing);
            problemDetails.Title.Should().Be(ReasonPhrases.GetReasonPhrase(StatusCodes.Status102Processing));
        }

        public sealed class CustomException(string message) : Exception(message);

        private static ProblemDetails DefaultException(Exception exception, HttpContext context, string? errorMessage)
        {
            if (exception is not CustomException customException)
            {
                return ExceptionToProblemDetailsMapperService.DefaultException(exception, context, errorMessage);
            }

            return new ProblemDetails
            {
                Detail = !string.IsNullOrEmpty(errorMessage) ? $"Processing-{errorMessage};{customException.Message}" : $"Processing-{customException.Message}",
                Instance = context.Request.Path,
                Status = StatusCodes.Status102Processing,
                Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status102Processing)
            };
        }
    }

    private static DefaultHttpContext CreateHttpContext(string requestPath) =>
        new() { Request = { Path = requestPath } };
}
