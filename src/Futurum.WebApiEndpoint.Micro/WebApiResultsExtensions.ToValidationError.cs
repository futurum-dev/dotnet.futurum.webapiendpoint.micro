using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Futurum.WebApiEndpoint.Micro;

public static partial class WebApiResultsExtensions
{
    public static Option<ValidationProblem> ToValidationProblem(IResultError resultError, HttpContext context)
    {
        if (resultError is ResultErrorValidation resultErrorValidation)
        {
            var errors = resultErrorValidation.ValidationErrors.ToDictionary(x => x.PropertyName, x => x.ErrorMessages.ToArray());

            var validationProblem = TypedResults.ValidationProblem(errors, instance: context.Request.Path);

            return validationProblem.ToOption();
        }

        return Option.None<ValidationProblem>();
    }
}