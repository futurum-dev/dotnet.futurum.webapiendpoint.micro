using System.ComponentModel.DataAnnotations;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Micro;

public interface IDataAnnotationsValidationService
{
    Result Execute<T>(T instance)
        where T : class;
}

public class DataAnnotationsValidationService : IDataAnnotationsValidationService
{
    public Result Execute<T>(T instance)
        where T : class
    {
        var context = new ValidationContext(instance);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(instance, context, validationResults, true);

        if (isValid)
        {
            return Result.Ok();
        }

        var validationErrors = validationResults
                               .Where(validationResult => !string.IsNullOrEmpty(validationResult.ErrorMessage))
                               .SelectMany(validationResult => validationResult.MemberNames.Select(memberName => new ValidationError(memberName, new[] { validationResult.ErrorMessage! })));

        return Result.Fail(validationErrors.ToResultError());
    }
}