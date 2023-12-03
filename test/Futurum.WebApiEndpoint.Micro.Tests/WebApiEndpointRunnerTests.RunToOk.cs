using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointRunnerRunToOkTests
{
    private const string REQUEST_PATH = "/RequestPath";

    private const string ErrorMessage1 = "ERROR_MESSAGE1";
    private const string ErrorMessage2 = "ERROR_MESSAGE2";

    public class Sync
    {
        public class NoValue
        {
            public class ErrorMessage
            {
                [Fact]
                public void RunAsync_ReturnsExpectedResult()
                {
                    var results = WebApiEndpointRunner.RunToOk(() => {},
                                                               CreateHttpContext(),
                                                               ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = WebApiEndpointRunner.RunToOk( () =>
                                                                {
                                                                    throw new Exception(ErrorMessage1);
                                                                },
                                                                CreateHttpContext(),
                                                                ErrorMessage2);

                    ValidateBadRequest(results);
                }


                private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok>();
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void RunAsync_ReturnsExpectedResult()
                {
                    var results = WebApiEndpointRunner.RunToOk( () => {},
                                                                CreateHttpContext(),
                                                                () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = WebApiEndpointRunner.RunToOk( () =>
                                                                {
                                                                    throw new Exception(ErrorMessage1);
                                                                },
                                                                CreateHttpContext(),
                                                                () => ErrorMessage2);

                    ValidateBadRequest(results);
                }

                private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok>();
                }
            }
        }

        public class Value
        {
            public class ErrorMessage
            {
                [Fact]
                public void RunAsync_ReturnsExpectedResult()
                {
                    var input = Guid.NewGuid();

                    var results = WebApiEndpointRunner.RunToOk(() => input,
                                                               CreateHttpContext(),
                                                               ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = WebApiEndpointRunner.RunToOk( () =>
                                                                {
                                                                    throw new Exception(ErrorMessage1);

                                                                    return TypedResults.Ok();
                                                                },
                                                                CreateHttpContext(),
                                                                ErrorMessage2);

                    ValidateBadRequest(results);
                }


                private static void ValidateBadRequest<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok<T>>();
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void RunAsync_ReturnsExpectedResult()
                {
                    var input = Guid.NewGuid();

                    var results = WebApiEndpointRunner.RunToOk( () => input,
                                                                CreateHttpContext(),
                                                                () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = WebApiEndpointRunner.RunToOk( () =>
                                                                {
                                                                    throw new Exception(ErrorMessage1);

                                                                    return TypedResults.Ok();
                                                                },
                                                                CreateHttpContext(),
                                                                () => ErrorMessage2);

                    ValidateBadRequest(results);
                }

                private static void ValidateBadRequest<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok<T>>();
                }
            }
        }
    }

    public class Async
    {
        public class NoValue
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task RunAsync_ReturnsExpectedResult()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () => {},
                                                                          CreateHttpContext(),
                                                                          ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () =>
                                                                          {
                                                                              throw new Exception(ErrorMessage1);
                                                                          },
                                                                          CreateHttpContext(),
                                                                          ErrorMessage2);

                    ValidateBadRequest(results);
                }


                private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok>();
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task RunAsync_ReturnsExpectedResult()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () => {},
                                                                          CreateHttpContext(),
                                                                          () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () =>
                                                                          {
                                                                              throw new Exception(ErrorMessage1);
                                                                          },
                                                                          CreateHttpContext(),
                                                                          () => ErrorMessage2);

                    ValidateBadRequest(results);
                }

                private static void ValidateBadRequest(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk(Results<Ok, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok>();
                }
            }
        }

        public class Value
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task RunAsync_ReturnsExpectedResult()
                {
                    var input = Guid.NewGuid();

                    var results = await WebApiEndpointRunner.RunToOkAsync(async () => input,
                                                                          CreateHttpContext(),
                                                                          ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () =>
                                                                          {
                                                                              throw new Exception(ErrorMessage1);

                                                                              return TypedResults.Ok();
                                                                          },
                                                                          CreateHttpContext(),
                                                                          ErrorMessage2);

                    ValidateBadRequest(results);
                }


                private static void ValidateBadRequest<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok<T>>();
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task RunAsync_ReturnsExpectedResult()
                {
                    var input = Guid.NewGuid();

                    var results = await WebApiEndpointRunner.RunToOkAsync(async () => input,
                                                                          CreateHttpContext(),
                                                                          () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task RunAsync_ThrowsException_ReturnsBadRequest()
                {
                    var results = await WebApiEndpointRunner.RunToOkAsync(async () =>
                                                                          {
                                                                              throw new Exception(ErrorMessage1);

                                                                              return TypedResults.Ok();
                                                                          },
                                                                          CreateHttpContext(),
                                                                          () => ErrorMessage2);

                    ValidateBadRequest(results);
                }

                private static void ValidateBadRequest<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                    results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                    results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
                }

                private static void ValidateOk<T>(Results<Ok<T>, BadRequest<ProblemDetails>> results)
                {
                    results.Result.Should().BeOfType<Ok<T>>();
                }
            }
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}
