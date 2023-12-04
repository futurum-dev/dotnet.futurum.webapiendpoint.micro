using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class WebApiEndpointRunnerRunTests
{
    private const string REQUEST_PATH = "/RequestPath";

    private const string ErrorMessage1 = "ERROR_MESSAGE1";
    private const string ErrorMessage2 = "ERROR_MESSAGE2";

    private const string VALUE = "Value";

    public class Sync
    {
        public class IResult1
        {
            public class ErrorMessage
            {
                [Fact]
                public void result_success()
                {
                    var input = TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return TypedResults.Ok();
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
                public void result_success()
                {
                    var input = TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return TypedResults.Ok();
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

        public class IResult2
        {
            public class ErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateOk(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult3
        {
            public class ErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult4
        {
            public class ErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Created>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult5
        {
            public class ErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public void result5_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateConflict(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public void result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public void result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public void result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public void result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public void result5_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                    var results = WebApiEndpointRunner.Run(() => input,
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateConflict(results);
                }

                [Fact]
                public void Exception()
                {
                    var results = WebApiEndpointRunner.Run(() =>
                                                           {
                                                               throw new Exception(ErrorMessage1);

                                                               return (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();
                                                           },
                                                           CreateHttpContext(),
                                                           () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Created>();
            }

            private static void ValidateConflict(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Conflict>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }
    }

    public class Async
    {
        public class IResult1
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task result_success()
                {
                    var input = TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return TypedResults.Ok();
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
                public async Task result_success()
                {
                    var input = TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return TypedResults.Ok();
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

        public class IResult2
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateOk(Results<Ok, NotFound, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult3
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult4
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created>)TypedResults.Created(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted, Created>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Created>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }

        public class IResult5
        {
            public class ErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public async Task result5_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateConflict(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            public class FuncErrorMessage
            {
                [Fact]
                public async Task result1_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateOk(results);
                }

                [Fact]
                public async Task result2_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.NotFound();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateNotFound(results);
                }

                [Fact]
                public async Task result3_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Accepted(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateAccepted(results);
                }

                [Fact]
                public async Task result4_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Created(VALUE);

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateCreated(results);
                }

                [Fact]
                public async Task result5_success()
                {
                    var input = (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Conflict();

                    var results = await WebApiEndpointRunner.RunAsync(async () => input,
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateConflict(results);
                }

                [Fact]
                public async Task Exception()
                {
                    var results = await WebApiEndpointRunner.RunAsync(async () =>
                                                                      {
                                                                          throw new Exception(ErrorMessage1);

                                                                          return (Results<Ok, NotFound, Accepted, Created, Conflict>)TypedResults.Ok();
                                                                      },
                                                                      CreateHttpContext(),
                                                                      () => ErrorMessage2);

                    ValidateBadRequest(results);
                }
            }

            private static void ValidateBadRequest(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<BadRequest<ProblemDetails>>();
                results.Result.As<BadRequest<ProblemDetails>>().StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
                results.Result.As<BadRequest<ProblemDetails>>().Value.Detail.Should().Be($"{ErrorMessage2};{ErrorMessage1}");
                results.Result.As<BadRequest<ProblemDetails>>().Value.Instance.Should().Be(REQUEST_PATH);
            }

            private static void ValidateNotFound(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<NotFound>();
            }

            private static void ValidateAccepted(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Accepted>();
            }

            private static void ValidateCreated(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Created>();
            }

            private static void ValidateConflict(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Conflict>();
            }

            private static void ValidateOk(Results<Ok, NotFound, Accepted, Created, Conflict, BadRequest<ProblemDetails>> results)
            {
                results.Result.Should().BeOfType<Ok>();
            }
        }
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new() { Request = { Path = REQUEST_PATH } };
}
